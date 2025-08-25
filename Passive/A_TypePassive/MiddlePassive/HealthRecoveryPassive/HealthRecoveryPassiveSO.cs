using FMODUnity;
using KHJ.Shared;
using Main.Runtime.Combat;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "HealthRecoveryPassiveSO", menuName = "SO/Passive/Middle/HealthRecoveryPassive")]
    public class HealthRecoveryPassiveSO : PassiveSO, IActivePassive, ICooldownPassive, IEffectPoolPassive
    {
        public EventReference sound;
        [Range(1, 100)][SerializeField] private float _percent;
        [Range(1, 100)][SerializeField] private float _increaseHelthPercent;
        private float PercentAsRatio => _percent * 0.01f;
        private float IncreaseHeltPercentAsRatio => _increaseHelthPercent * 0.01f;


        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.OnHitTarget -= HandleHitEvent;
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
            Health playerHP = _player.HealthCompo;
            if (playerHP.CurrentHealth < playerHP.MaxHealth * PercentAsRatio)
            {
                playerHP.CurrentHealth += playerHP.MaxHealth * IncreaseHeltPercentAsRatio;
                RuntimeManager.PlayOneShot(sound);
                _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            }
        }
    }
}