using BIS.Data;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using System;
using System.Linq;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "MillionairePassiveSO", menuName = "SO/Passive/Middle/MillionairePassiveSO")]
    public class MillionairePassiveSO : PassiveSO, IModifierStatPassive
    {
        [SerializeField] private CurrencySO _moneySO;
        [SerializeField] private int maxIncreaseStatValue;
        [field:SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _moneySO.ValueChangeEvent += HandleChangeValueEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _moneySO.ValueChangeEvent -= HandleChangeValueEvent;
        }

        private void HandleChangeValueEvent(int value)
        {
            ModifierStatInfo?.RemoveModifierEvent(ModifierStatInfo);            

            int increaseValue = (int)(value * 0.01f);
            int limitValue = Mathf.Clamp(increaseValue, 0, maxIncreaseStatValue);

            ModifierStatInfo.ModifierStats.ToList().ForEach(x => x.modifierValue = limitValue);
            ModifierStatInfo?.AddModifierEvent(ModifierStatInfo);
        }
    }
}
