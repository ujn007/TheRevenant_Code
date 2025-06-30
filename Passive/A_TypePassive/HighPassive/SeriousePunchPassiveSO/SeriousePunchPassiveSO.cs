using Main.Runtime.Agents;
using Main.Runtime.Core.Events;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "SeriousePunchPassiveSO", menuName = "SO/Passive/High/SeriousePunchPassiveSO")]
    public class SeriousePunchPassiveSO : PassiveSO, IActivePassive, ICooldownPassive, IEffectPoolPassive
    {
        [SerializeField] private float _radius;
        [SerializeField] private StatSO _attackPowerStatSO;
        [SerializeField] private int _increasePercent;
        private float PercentRatio => _increasePercent * 0.01f;

        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player = player as Player;
        }

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
            Agent target = info.hitTarget as Agent;
            float value = _player.GetCompo<PlayerStat>().GetStat(_attackPowerStatSO).Value;
            float newValue = value + (value * PercentRatio);

            FindEnemiesInRange(target.transform, _radius).ForEach(enemy => enemy.HealthCompo.ApplyOnlyDamage(newValue));
            CooldownPassiveInfo.StartCooldownEvent?.Invoke();

            var evt = SpawnEvents.SpawnEffect;
            evt.position = target.transform.position + Vector3.up;
            evt.rotation = Quaternion.identity;
            evt.effectType = PoolType;
            _spawnEventChannel.RaiseEvent(evt);
        }
    }
}
