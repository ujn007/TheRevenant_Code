using System;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "QuickExecutionPassiveSO",
        menuName = "SO/Passive/Middle/QuickExecutionPassiveSO")]
    public class QuickExecutionPassiveSO : PassiveSO, IActivePassive, IModifierStatPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
        }

        public void ActivePassive()
        {
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }

        public void DeActivePassive()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}