using FMODUnity;
using KHJ.Shared;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "InvincibilityChancePassiveSO", menuName = "SO/Passive/Middle/InvincibilityChancePassiveSO")]
    public class InvincibilityChancePassiveSO : PassiveSO, IActivePassive, IBuffPassive, ICooldownPassive, IEffectPoolPassive
    {
        public EventReference sound;
        [SerializeField] private int _invincibilityPercent;

        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
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

        public void EndBuff()
        {
            _player.HealthCompo.IsInvincibility = false;
        }

        public void StartBuff()
        {
            _player.HealthCompo.IsInvincibility = true;
        }

        private void HandleHitEvent(HitInfo info)
        {
            int rand = Random.Range(1, 101);
            if (rand < _invincibilityPercent)
            {
                BuffPassiveInfo.ApplyBuffEvent?.Invoke();
                RuntimeManager.PlayOneShot(sound);
                _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            }
        }
    }
}
