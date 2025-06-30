using DG.Tweening;
using KHJ.Enum;
using KHJ.SO;
using Main.Runtime.Core.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Main.Core;
using Main.Runtime.Manager;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace KHJ
{
    public class SynergyBlock : MonoBehaviour
    {
        [TabGroup("SO")] public SynergySO synergySO;
        private GameEventChannelSO _gameEventChannel;

        [TabGroup("Synergy")] [SerializeField] private SynergyBlockParts sblock;
        [TabGroup("Synergy")] [SerializeField] private VirtualParts virtualParts;
        [TabGroup("Synergy")] [SerializeField] private Image outLineImg;
        [TabGroup("Synergy")] [SerializeField] private RectTransform outLineCon;
        [field: SerializeField] public List<SynergyBlockParts> partsList { get; private set; }
        private List<RectTransform> outLineList = new();

        private SynergyUIManager uiManager => SynergyUIManager.Instance;
        private SynergyBoardManager boardManager => SynergyBoardManager.Instance;

        private RectTransform rectTrm => transform as RectTransform;
        public Vector2Int coord => uiManager.GetGridPos(virtualParts.transform.position);

        [field: SerializeField] public SynergyBlockParts currentParts { get; set; }

        private Transform beforeParent;
        private Vector2 beforePos;
        private Vector2Int beforeGridPos;

        private Vector2 mouseDir;

        public bool isImage { get; set; } = false;
        [field: SerializeField] public bool isMainInven { get; private set; }
        private bool isServeInven;

        private bool isDrag = false;
        public bool previousState { get; set; }

        private void Awake()
        {
            _gameEventChannel = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
        }

        private void Update()
        {
            if (isImage) return;
            DragFollow();
            ChangeSize();
        }

        public void Init()
        {
            SetUpParts();
            currentParts = partsList[0];
            isMainInven = true;
            SetServeImg(false);
            partsList.ForEach(s => s.Initialize(this, synergySO.grade));
        }

        private void SetUpParts()
        {
            partsList.Clear();

            List<Vector2Int> spaceDir = synergySO.spaces;

            for (int i = 0; i < spaceDir.Count; i++)
            {
                Vector2Int dir = spaceDir[i];
                SynergyBlockParts parts = Instantiate(sblock, transform);
                Image outLine = Instantiate(outLineImg, outLineCon);
                parts.name += $"_{i + 1}";

                RectTransform trm = parts.gameObject.transform as RectTransform;
                trm.anchoredPosition = trm.sizeDelta * dir;
                outLine.rectTransform.anchoredPosition = trm.anchoredPosition;

                parts.image.color = synergySO.typeColor;
                parts.girdPos = dir;

                partsList.Add(parts);
            }

            virtualParts.SetGradeAndColor(synergySO.grade, synergySO.typeColor);

            foreach (RectTransform line in outLineCon)
                outLineList.Add(line);
        }

        private void DragFollow()
        {
            if (!isDrag) return;
            rectTrm.position = uiManager.GetMousePos() - mouseDir;
            //    boardManager.SetColorBlockPos(synergySO.spaces, GetCenterGridSlot(), !uiManager.IsWithinGridBounds(currentParts.transform.position));
        }

        public void PointerDown(SynergyBlockParts block, SynergySlotUI slot)
        {
            currentParts = block;

            beforeParent = transform.parent;
            beforePos = slot.isServeSlot ? slot.transform.position : PosToSlot(GetCenterGridSlot());
            if (slot.isServeSlot) SetServeImg(false);
            beforeGridPos = slot.isServeSlot ? uiManager.GetGridPos() : GetCenterGridSlot();
            isServeInven = false;

            transform.SetParent(uiManager.synergyBlockHandler.transform);
            mouseDir = uiManager.GetMousePos() - (Vector2)rectTrm.position;

            if (!slot.isServeSlot) SetBefore(SynergyType.Empty);
            else SetServeBefore(SynergyType.Empty);

            boardManager.InitParts(block);

            StatInfo();

            isDrag = true;
        }

        public void PointerUp(SynergyBlockParts block, SynergySlotUI slot)
        {
            if (!beforeParent) return;
            transform.SetParent(uiManager.blockContainer);
            isDrag = false;

            boardManager.InitParts(null);

            bool isWithinBounds = uiManager.IsWithinGridBounds(currentParts.transform.position);
            bool isMainSet = isWithinBounds &&
                             boardManager.SetSynergyBlock(partsList, synergySO.spaces, GetCenterGridSlot(),
                                 synergySO.synergyType);
            bool isSubSet = !isWithinBounds && slot != null &&
                            boardManager.SetServeBlock(currentParts, uiManager.GetGridPos().y, synergySO.synergyType);

            if (isMainSet)
            {
                rectTrm.position = PosToSlot(GetCenterGridSlot());
                StatIn(true);
                isMainInven = true;
            }
            else if (isSubSet)
            {
                SetServeSlot(uiManager.GetGridPos().y);
                isServeInven = true;
            }
            else
            {
                rectTrm.position = beforePos;
                transform.SetParent(beforeParent);
                if (isMainInven) SetBefore(synergySO.synergyType);
                else SetServeBefore(synergySO.synergyType);
                _gameEventChannel.RaiseEvent(GameEvents.ShakeInven);
            }

            boardManager.DetectTriangle();
        }

        public void SetServeSlot(int y)
        {
            SetServeImg(true);
            StatIn(false);
            rectTrm.position = boardManager.serveBoard[y].transform.position;
            isMainInven = false;
        }

        private void SetServeImg(bool isSet)
        {
            if (!isSet) rectTrm.position += virtualParts.transform.position - currentParts.transform.position;

            print($"{synergySO.name} : {isSet}");
            partsList.ForEach(x => x.OnEnableAllImg(!isSet));
            OnEnableOutLine(!isSet);
            virtualParts.gameObject.SetActive(isSet);
        }

        public Vector2Int GetCenterGridSlot()
        {
            Vector2 center = partsList[0].transform.position;
            Vector2Int centerPos = uiManager.GetGridPos(center);
            return centerPos;
        }

        public Vector2Int GetCurrentGridSlot() => uiManager.GetGridPos(currentParts.transform.position);

        private void ChangeSize()
        {
            RectTransform currentP = currentParts.transform as RectTransform;
            if (isServeInven) currentP = rectTrm;
            bool currentState = uiManager.IsWithinGridBounds(currentP.position);

            if (currentState != previousState)
            {
                List<Vector2Int> spaceDir = synergySO.spaces;

                if (!currentState)
                {
                    for (int i = 0; i < partsList.Count; i++)
                    {
                        RectTransform rcTrm = partsList[i].transform as RectTransform;
                        rcTrm.DOAnchorPos(currentP.anchoredPosition, 0.1f);
                        outLineList[i].DOAnchorPos(currentP.anchoredPosition, 0.1f);
                    }
                }
                else
                {
                    for (int i = 0; i < spaceDir.Count; i++)
                    {
                        Transform parts = partsList[i].transform;
                        RectTransform trm = parts.gameObject.transform as RectTransform;
                        trm.DOAnchorPos(trm.sizeDelta * spaceDir[i], 0.1f);
                        outLineList[i].DOAnchorPos(trm.sizeDelta * spaceDir[i], 0.1f);
                    }
                }

                previousState = currentState;
            }
        }

        private bool prevIsIn;

        public void StatIn(bool isIn)
        {
            if (isIn != prevIsIn)
            {
                if (isIn)
                    IncreaseStat();
                else
                    DecreaseStat();

                prevIsIn = isIn;
            }
        }

        private Vector3 PosToSlot(Vector2Int pos) => boardManager.slotBoard[pos.x, pos.y].transform.position;

        public void SetGridPos(int i, int j) => rectTrm.position = boardManager.slotBoard[i, j].transform.position;
        public void SetServeGridPos(int j) => rectTrm.position = boardManager.serveBoard[j].transform.position;

        private void OnEnableOutLine(bool isEnable)
        {
            outLineList.ForEach(x => x.gameObject.SetActive(isEnable));
        }

        private bool SetBefore(SynergyType type) =>
            boardManager.SetSynergyBlock(partsList, synergySO.spaces, beforeGridPos, type);

        private bool SetServeBefore(SynergyType type)
        {
            if (type != SynergyType.Empty) SetServeImg(true);
            partsList.ForEach(x => x.OnEnableGradeAndOutLine(false));
            return boardManager.SetServeBlock(currentParts, beforeGridPos.y, type);
        }

        public SynergyBlockParts GetCurrentParts(string name) =>
            partsList.FirstOrDefault(parts => parts.gameObject.name == name);

        #region Player Stat

        private void StatInfo()
        {
            var evt = GameEvents.CurrentBlockInfo;
            evt.stat = synergySO.increasePlayerStat;
            evt.statValue = Mathf.Round(synergySO.increasePlayerStatValue * 10f) / 10f;
            evt.grade = synergySO.grade;
            _gameEventChannel.RaiseEvent(evt);
        }

        public void IncreaseStat()
        {
            var evt = GameEvents.ModifyPlayerStat;
            evt.modifyPlayerStat = synergySO.increasePlayerStat;
            evt.modifyPlayerStatValue = Mathf.Round(synergySO.increasePlayerStatValue * 10f) / 10f;
            evt.modifyKey = this;
            evt.isIncreaseStat = true;
            Debug.LogError($"adadd : {evt.isIncreaseStat}");
            _gameEventChannel.RaiseEvent(evt);
        }

        public void ModifyStat(float modify)
        {
            var evt = GameEvents.ModifyPlayerStat;
            evt.modifyPlayerStat = synergySO.increasePlayerStat;
            float value = Mathf.Round(synergySO.increasePlayerStatValue * 10f) / 10f;
            evt.modifyPlayerStatValue = (value * modify) - value;
            evt.modifyKey = this;
            evt.isIncreaseStat = true;
            _gameEventChannel.RaiseEvent(evt);
        }

        public void DecreaseStat()
        {
            var evt = GameEvents.ModifyPlayerStat;
            evt.modifyPlayerStat = synergySO.increasePlayerStat;
            evt.modifyKey = this;
            evt.isIncreaseStat = false;
            _gameEventChannel.RaiseEvent(evt);
        }

        #endregion
    }
}