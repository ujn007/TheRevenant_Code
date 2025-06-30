using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KHJ
{
    public class SynergyServeContainer : MonoBehaviour
    {
        private List<SynergySlotUI> slotList = new();
        public static SynergyServeContainer Instance;

        private void Awake()
        {
            Instance = this;
            slotList = GetComponentsInChildren<SynergySlotUI>().ToList();
        }

        public void CheckServeSlot()
        {
            foreach (SynergySlotUI slot in slotList)
            {
                if (slot.parts != null)
                {
                    SynergyBlock block = slot.GetComponentInParent<SynergyBlock>();
                    block.DecreaseStat();
                }
            }
        }
    }
}
