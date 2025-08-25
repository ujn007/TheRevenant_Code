using FMODUnity;
using KHJ.Shared;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BoilingBloodPassiveSO", menuName = "SO/Passive/Middle/BoilingBloodPassiveSO")]
    public class BoilingBloodPassiveSO : PassiveSO, IModifierStatPassive, IBuffPassive, ICooldownPassive
    {
        public EventReference sound;    
        [SerializeField] private int _healthPercent;
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }

        private float PercentRatio => _healthPercent * 0.01f;
        private bool isStop = false;

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
            RuntimeManager.PlayOneShot(sound);
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }

        public void EndBuff()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
