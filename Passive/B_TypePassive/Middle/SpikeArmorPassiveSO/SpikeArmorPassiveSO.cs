using Main.Runtime.Combat;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "SpikeArmorPassiveSO", menuName = "SO/Passive/Middle/SpikeArmorPassiveSO")]
    public class SpikeArmorPassiveSO : PassiveSO
    {
        [SerializeField] private float _returnDamagePercent;
        private float PercentRatio => _returnDamagePercent * 0.01f;

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.HealthCompo.OnApplyDamaged += HandleDamagedEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.HealthCompo.OnApplyDamaged -= HandleDamagedEvent;
        }

        private void HandleDamagedEvent(float damage)
        {
            Health targetHealth = _player.HealthCompo.GetDamagedInfo.attacker.GetComponent<Health>();
            targetHealth.ApplyOnlyDamage(targetHealth.MaxHealth * PercentRatio);
        }
    }
}
