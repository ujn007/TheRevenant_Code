using KHJ.Shared;
using Main.Runtime.Agents;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "OpenWoundPassiveSO", menuName = "SO/Passive/Middle/OpenWoundPassiveSO")]
    public class OpenWoundPassiveSO : PassiveSO, IActivePassive, IEffectPoolPassive
    {
        [SerializeField] private StatSO _powerStatSO;
        [SerializeField] private int _increasePercent;

        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        private float IncPercRatio => _increasePercent * 0.01f;

        public void ActivePassive()
        {
            _player.OnHitTarget += HandleHitEvent;
        }

        public void DeActivePassive()
        {
            _player.OnHitTarget -= HandleHitEvent;
        }

        private void HandleHitEvent(HitInfo info)
        {
            Agent enemy = info.hitTarget as Agent;
            Vector3 hitPoint = enemy.HealthCompo.GetDamagedInfo.hitPoint;
            float currentValue = _player.GetCompo<PlayerStat>().GetStat(_powerStatSO).Value;

            if (enemy.HealthCompo.ailmentStat.HasAilment(Ailment.Dot))
            {
                enemy.HealthCompo.ApplyOnlyDamage(currentValue * IncPercRatio);
                PlayEffect(PoolType, hitPoint, Quaternion.identity);
            }
        }
    }
}
