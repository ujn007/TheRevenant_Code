using DG.Tweening;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using Main.Core;
using Main.Runtime.Agents;
using Main.Runtime.Manager;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace KHJ.UI
{
    public enum MoveDirType
    {
        None,
        Right,
        Left
    }

    [System.Serializable]
    public struct IconInfo
    {
        public Vector2 iconPos;
        public float scale;
        public bool isActive;
    }

    public class CurrentComboUI : MonoBehaviour
    {
        private PlayerInputSO inputSO;
        [SerializeField] private CurrentComboIcon iconPF;
        [SerializeField] private float tweenDuration;
        [SerializeField] private List<IconInfo> iconInfoList = new();

        private List<CurrentComboIcon> comboIconList = new();
        private int currentIndex = 1;

        private Agent _player;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (!_player) return;
            PlayerCommandActionManager commandActionManager = _player.GetCompo<PlayerCommandActionManager>();

            commandActionManager.OnUseCommandActionIndex += HandleChangeComnboEvent;
        }

        private void UnsubscribeEvents()
        {
            if (!_player) return;
            PlayerCommandActionManager commandActionManager = _player.GetCompo<PlayerCommandActionManager>();

            commandActionManager.OnUseCommandActionIndex -= HandleChangeComnboEvent;
        }

        private IEnumerator Start()
        {
            Initialize();
            yield return new WaitUntil(() => PlayerManager.Instance.Player);
            _player = PlayerManager.Instance.Player;

            SubscribeEvents();
        }

        private void Initialize()
        {
            inputSO = AddressableManager.Load<PlayerInputSO>("PlayerInputSO");
            for (int i = 0; i < iconInfoList.Count; i++)
            {
                CurrentComboIcon icon = Instantiate(iconPF, transform);
                icon.gameObject.name = "ComboIcon" + (i + 1);
                icon.SetText(i + 1);
                comboIconList.Add(icon);
                ChangeIconInfo(icon, i);
            }

            InputIconKey(0);
        }

        private void HandleChangeComnboEvent(int index)
        {
            InputIconKey(index);
        }

        private void InputIconKey(int index)
        {
            MoveDirType type = LeftOrRight(index);
            if (MoveDirType.None == type) return;

            MoveIcons(type);
            currentIndex = index;
        }

        private void MoveIcons(MoveDirType type)
        {
            for (int i = 0; i < iconInfoList.Count; i++)
            {
                CurrentComboIcon icon = comboIconList[i];
                int nextindex = GetNextIndex(i, type);
                ChangeIconInfo(icon, nextindex);
            }

            CycleArray(type);
        }

        private void ChangeIconInfo(CurrentComboIcon icon, int index)
        {
            IconInfo info = iconInfoList[index];
            icon.rect.DOAnchorPos(info.iconPos, tweenDuration);
            icon.rect.DOScale(Vector3.one * info.scale, tweenDuration);
            icon.SetDOFade(info.isActive, tweenDuration);
        }

        private void CycleArray(MoveDirType type)
        {
            switch (type)
            {
                case MoveDirType.Left:
                {
                    CurrentComboIcon first = comboIconList[0];
                    comboIconList.RemoveAt(0);
                    comboIconList.Add(first);
                    break;
                }
                case MoveDirType.Right:
                {
                    {
                        CurrentComboIcon last = comboIconList[comboIconList.Count - 1];
                        comboIconList.RemoveAt(comboIconList.Count - 1);
                        comboIconList.Insert(0, last);
                        break;
                    }
                }
            }
        }

        private int GetNextIndex(int curIndex, MoveDirType type)
        {
            switch (type)
            {
                case MoveDirType.Left:
                    return (curIndex == 0) ? 2 : curIndex - 1;
                case MoveDirType.Right:
                    return (curIndex == 2) ? 0 : curIndex + 1;
                default:
                    return curIndex;
            }
        }

        private MoveDirType LeftOrRight(int index)
        {
            if (currentIndex == index)
                return MoveDirType.None;

            foreach (MoveDirType type in Enum.GetValues(typeof(MoveDirType)))
            {
                if (type == MoveDirType.None) continue;
                if (GetNextIndex(currentIndex, type) != index)
                    return type;
            }

            Debug.LogError("ComboUi Error!!");
            return MoveDirType.Right;
        }
    }
}