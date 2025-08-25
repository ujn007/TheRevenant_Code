using KHJ.Shared;
using Main.Runtime.Agents;
using Main.Runtime.Core.Events;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using Sirenix.Serialization;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Debug = Main.Core.Debug;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "FrozenPassiveSO", menuName = "SO/Passive/High/FrozenPassiveSO")]
    public class FrozenPassiveSO : PassiveSO, ICooldownPassive, IActivePassive, IEffectPoolPassive
    {
        public EventReference frozenSound;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _freezeDuration;
        private float _freezeValue = 100f;

        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

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
            List<Agent> enemyFinds = FindEnemiesInRange(enemy.transform, _attackRadius);
            enemyFinds.ForEach(FreezeTarget);
            FreezeTarget(enemy);
            PlayEffect(enemy, enemyFinds);
            CooldownPassiveInfo.StartCooldownEvent?.Invoke();
        }

        private void FreezeTarget(Agent target)
        {
            RuntimeManager.PlayOneShot(frozenSound, target.transform.position);
            target.HealthCompo.ailmentStat.ApplyAilments(Ailment.Slow, _freezeDuration, _freezeValue);
        }

        private void PlayEffect(Agent enemy, List<Agent> enemyFinds)
        {
            Debug.Log($"애너미 개수 : {enemyFinds.Count}");
            foreach (Agent enem in enemyFinds)
            {
                Vector3 startPos = enemy.transform.position;
                Vector3 targetPos = enem.transform.position;
                Vector3 dir = (targetPos - startPos).normalized;
                float distance = Vector3.Distance(startPos, targetPos);

                var evt = SpawnEvents.SpawnEffect;
                evt.effectType = PoolType;
                evt.position = enemy.transform.position;
                evt.rotation = Quaternion.LookRotation(dir);

                float particleSpeed = 40f;
                float lifetime = distance / particleSpeed;

                evt.lifeTime = enemy != enem ? lifetime : 0.01f;
                _spawnEventChannel.RaiseEvent(evt);
            }
        }
    }
}