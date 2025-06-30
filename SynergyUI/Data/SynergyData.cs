using System.Collections.Generic;
using UnityEngine;

namespace KHJ.Data
{
    [System.Serializable]
    public struct SynergyData
    {
        public List<BlockData> blockDatas;
    }

    [System.Serializable]
    public struct BlockData
    {
        public BlockData(Vector2Int v, string n, bool isMain, string name)
        {
            coord = v;
            this.id = n;
            isMainInven = isMain;
            partsName = name;
        }

        public Vector2Int coord;
        public string id;
        public bool isMainInven;
        public string partsName;
    }
}
