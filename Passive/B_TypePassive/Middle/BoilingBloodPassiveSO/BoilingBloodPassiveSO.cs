using Main.Shared;
using PJH.Runtime.PlayerPassive;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BoilingBloodPassiveSO", menuName = "SO/Passive/Middle/BoilingBloodPassiveSO")]
    public class BoilingBloodPassiveSO : PassiveSO, IModifierStatPassive, IBuffPassive, ICooldownPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [SerializeField] private int _healthPercent;
        private float PercentRatio => _healthPercent * 0.01f;

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.HealthCompo.OnChangedHealth += HandleChangeHealthEvent;
        }

        private void HandleChangeHealthEvent(float currentHealth, float minHealth, float maxHealth)
        {
            if (currentHealth <= maxHealth * PercentRatio)
            {
                BuffPassiveInfo.ApplyBuffEvent?.Invoke();
                CooldownPassiveInfo.StartCooldownEvent?.Invoke();
            }
        }

        public void StartBuff()
        {
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }

        public void EndBuff()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
