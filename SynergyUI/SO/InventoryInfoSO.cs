using System.Collections.Generic;
using UnityEngine;

namespace KHJ.SO
{
    [CreateAssetMenu(fileName = "InventoryInfoSO", menuName = "SO/Synergy/InvenSO")]
    public class InventoryInfoSO : ScriptableObject
    {
        public int childchildCount;
        public int childCount;
        public RectTransform fistSlot;
        public RectTransform endSlot;
    }
}

