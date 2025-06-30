using BIS.Data;
using BIS.Shared.Interface;
using KHJ.Data;
using KHJ.Enum;
using KHJ.SO;
using System.Collections.Generic;
using UnityEngine;

namespace KHJ
{
    public class SynergyBoardManager : MonoBehaviour , ISavable
    {
        [SerializeField] private InventoryInfoSO inventoryInfoSO;
        [SerializeField] private SynergyListSO synergyListSO;
        [SerializeField] private SynergyBlock synergyBlock;
        [field: SerializeField] public SaveIDSO IdData { get; set; }

        private SynergyUIManager uiManager => SynergyUIManager.Instance;

        public SynergySlotUI[,] slotBoard { get; private set; }
        public SynergyType[,] synergyBoard { get; private set; }

        public SynergySlotUI[] serveBoard { get; private set; }
        public SynergyType[] synergyServeBoard { get; private set; }

        private int rangeX => 12;
        private int rangeY => 10;

        private List<SynergyBlock> detectSynergyBlockList = new();
        private List<SynergyBlock> blockList = new();
        private Vector2Int beforeCoord = Vector2Int.down;
        private bool isGameStart;

        public static SynergyBoardManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Debug.LogWarning(rangeX);
            Debug.LogWarning(rangeY);
            slotBoard = new SynergySlotUI[rangeX, rangeY];
            synergyBoard = new SynergyType[rangeX, rangeY];

            serveBoard = new SynergySlotUI[rangeY];
            synergyServeBoard = new SynergyType[rangeY];

            SetPoses();
        }

        private void OnEnable()
        {
            DetectTriangle();
        }

        //private Vector2Int before;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                blockList.ForEach(x => x.DecreaseStat());
            }
        }

        private void SetPoses()
        {
            for (int i = 0; i < rangeX; i++)
            {
                for (int j = 0; j < rangeY; j++)
                {
                    Transform spaceTrm = uiManager.synergyContainer.transform.GetChild(i).GetChild(j);

                    if (spaceTrm.TryGetComponent(out SynergySlotUI synergySpace))
                    {
                        synergySpace.Initialize(uiManager.synergyBlockHandler, new Vector2Int(i, j));
                        slotBoard[i, j] = synergySpace;
                        synergyBoard[i, j] = SynergyType.Empty;
                    }
                }
            }

            for (int j = 0; j < rangeY; j++)
            {
                Transform childServe = uiManager.serveSynergyContainer.GetChild(0).GetChild(j);

                if (childServe.TryGetComponent(out SynergySlotUI synergySpace))
                {
                    synergySpace.IsServeSlot(true);
                    synergySpace.Initialize(uiManager.synergyBlockHandler, new Vector2Int(0, j));
                    serveBoard[j] = synergySpace;
                    synergyServeBoard[j] = SynergyType.Empty;
                }
            }
        }

        public bool SetFistSlotBlock(SynergySO blockSO, bool isMainInven, string currentPartName = null,
            Vector2Int coord = default)
        {
            SynergyBlock block = Instantiate(synergyBlock, uiManager.blockContainer);
            block.synergySO = blockSO;
            block.Init();
            blockList.Add(block);
            print(block.transform.parent.name);

            if (!isMainInven && coord != default)
            {
                for (int i = rangeY - 1; i >= 0; i--)
                {
                    block.currentParts = block.GetCurrentParts(currentPartName);
                    if (SetServeBlock(block.currentParts, coord.y, blockSO.synergyType))
                    {
                        block.previousState = true;
                        block.SetServeSlot(coord.y);
                        return true;
                    }
                }
            }

            if (coord != default)
            {
                if (SetSynergyBlock(block.partsList, blockSO.spaces, new Vector2Int(coord.x, coord.y),
                        blockSO.synergyType, true))
                {
                    block.SetGridPos(coord.x, coord.y);
                    block.StatIn(true);
                    return true;
                }
            }

            for (int i = rangeY - 1; i >= 0; i--)
            {
                for (int j = 0; j < rangeX; j++)
                {
                    if (SetSynergyBlock(block.partsList, blockSO.spaces, new Vector2Int(j, i),
                            blockSO.synergyType, true))
                    {
                        block.SetGridPos(j, i);
                        block.StatIn(true);
                        return true;
                    }
                }
            }

            Destroy(block.gameObject);
            return false;
        }

        public bool SetSynergyBlock(List<SynergyBlockParts> parts, List<Vector2Int> dir, Vector2Int coord,
            SynergyType type, bool isFirst = false)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (IsOutRange(coord + dir[i])) return false;
                if (!IsEmpty(coord + dir[i]) && type != SynergyType.Empty) return false;
                if (!isFirst && !uiManager.IsWithinGridBounds(parts[i].transform.position)) return false;
            }

            for (int i = 0; i < parts.Count; i++)
            {
                Vector2Int newCoord = coord + dir[i];
                synergyBoard[newCoord.x, newCoord.y] = type;

                slotBoard[newCoord.x, newCoord.y].parts = type == SynergyType.Empty ? null : parts[i];
            }

            SynergyBlockParts sy = type == SynergyType.Empty ? parts[0] : null;
            InitParts(sy);

            return true;
        }


        public bool SetServeBlock(SynergyBlockParts parts, int coordY, SynergyType type)
        {
            if (!IsEmptyServe(coordY) && type != SynergyType.Empty) return false;

            serveBoard[coordY].parts = parts;
            synergyServeBoard[coordY] = type;

            SynergyBlockParts sy = type == SynergyType.Empty ? parts : null;
            InitParts(sy);

            return true;
        }

        public void InitParts(SynergyBlockParts sy) => uiManager.synergyBlockHandler.parts = sy;

        public void SetColorBlockPos(List<Vector2Int> dir, Vector2Int coord, bool isOut)
        {
            if (beforeCoord == coord) return;

            if (beforeCoord == Vector2Int.down)
                beforeCoord = coord;

            print(coord);
            foreach (Vector2Int dirCoord in dir)
            {
                Vector2Int beforeCoord = this.beforeCoord + dirCoord;
                slotBoard[beforeCoord.x, beforeCoord.y].EnableMarkImage(false);
            }

            foreach (Vector2Int dirCoord in dir)
            {
                if (isOut || IsOutRange(coord + dirCoord) || !IsEmpty(coord + dirCoord))
                {
                    beforeCoord = Vector2Int.down;
                    return;
                }
            }

            foreach (Vector2Int dirCoord in dir)
            {
                Vector2Int newCoord = coord + dirCoord;
                slotBoard[newCoord.x, newCoord.y].EnableMarkImage(true);
            }

            this.beforeCoord = coord;
        }

        #region DetectTriangleZone

        public void DetectTriangle()
        {
            if (slotBoard == null)
                return;

            int x = slotBoard.GetLength(0);
            int y = slotBoard.GetLength(1);
            SynergyType beforeType = synergyBoard[1, 0];
            bool isFirst = true;

            DecreaseStat();
            detectSynergyBlockList.Clear();
            HashSet<SynergyBlock> blockSet = new();

            for (int i = 1; i < x / 2; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (!CheckZone(i, j, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = 0; i < y / 2; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (!CheckZone(j, i, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = x / 2; i < x; i++)
            {
                for (int j = 0; j <= y - i; j++)
                {
                    if (!CheckZone(i, j, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = 0; i < y / 2; i++)
            {
                for (int j = x - 1; j >= x - i - 1; j--)
                {
                    if (!CheckZone(j, i, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = 1; i < x / 2; i++)
            {
                for (int j = y - 1; j >= y - i; j--)
                {
                    if (!CheckZone(i, j, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = y / 2; i < y; i++)
            {
                for (int j = 0; j < y - i; j++)
                {
                    if (!CheckZone(j, i, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = x / 2; i < x - 1; i++)
            {
                for (int j = y - 1; j >= i - 1; j--)
                {
                    if (!CheckZone(i, j, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            isFirst = true;

            for (int i = y / 2; i < y; i++)
            {
                for (int j = x - 1; j > i + 1; j--)
                {
                    if (!CheckZone(j, i, ref beforeType, ref isFirst, ref blockSet)) continue;
                }
            }

            if (beforeType != SynergyType.Empty)
                detectSynergyBlockList.AddRange(blockSet);
            else
                blockSet.Clear();

            detectSynergyBlockList.ForEach(s => print(s.synergySO.synergyType));
            SynergyServeContainer.Instance.CheckServeSlot();
            ModifyStat();
        }

        private bool CheckZone(int i, int j, ref SynergyType beforeType, ref bool isFirst,
            ref HashSet<SynergyBlock> block)
        {
            beforeType = isFirst ? beforeType = synergyBoard[i, j] : beforeType;
            isFirst = false;

            if (beforeType == synergyBoard[i, j] && synergyBoard[i, j] != SynergyType.Empty)
            {
                beforeType = synergyBoard[i, j];

                Transform partsParent = slotBoard[i, j].parts.transform.parent;
                if (partsParent.TryGetComponent(out SynergyBlock part)) block.Add(part);
                return true;
            }
            else
            {
                beforeType = SynergyType.Empty;
                return false;
            }
        }

        private void ModifyStat() => detectSynergyBlockList.ForEach(s => s.ModifyStat(2));
        private void DecreaseStat() => detectSynergyBlockList.ForEach(s => s.DecreaseStat());

        #endregion

        private bool IsEmpty(Vector2Int coord)
        {
            Debug.LogWarning($"{synergyBoard.Length} : {coord.x}, {coord.y}");
            return synergyBoard[coord.x, coord.y] == SynergyType.Empty;
        }

        private bool IsEmptyServe(int coordY) => synergyServeBoard[coordY] == SynergyType.Empty;

        private bool IsOutRange(Vector2Int coord) =>
            rangeX <= coord.x || rangeY <= coord.y || 0 > coord.x || 0 > coord.y;

        public string GetSaveData()
        {
            List<BlockData> dataList = new();

            for (int i = 0; i < blockList.Count; i++)
            {
                dataList.Add(new BlockData(blockList[i].coord, blockList[i].synergySO.ItemID,
                  blockList[i].isMainInven, blockList[i].currentParts.gameObject.name));
            }

            blockList.ForEach(s => print(s.coord));

            SynergyData data = new SynergyData()
            {
                blockDatas = dataList,
            };
            return JsonUtility.ToJson(data);
        }

        public void RestoreData(string data)
        {
            if (!isGameStart) return;
            SynergyData synergyData = JsonUtility.FromJson<SynergyData>(data);

            for (int i = 0; i < synergyData.blockDatas.Count; i++)
            {
                BlockData blockD = synergyData.blockDatas[i];

                SynergySO so = synergyListSO.GetSynergySO(blockD.id);
                SetFistSlotBlock(so, blockD.isMainInven, blockD.partsName, blockD.coord);
            }
            isGameStart = true;
        }
    }
}