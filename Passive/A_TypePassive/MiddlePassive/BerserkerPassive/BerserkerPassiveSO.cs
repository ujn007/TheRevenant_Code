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
    [CreateAssetMenu(fileName = "BerserkerPassiveSO", menuName = "SO/Passive/Middle/BerserkerPassiveSO")]
    public class BerserkerPassiveSO : PassiveSO, IBuffPassive,
        ICooldownPassive, IModifierStatPassive, IEffectPoolPassive
    {
        public EventReference berserkerSound;
        [SerializeField] private float healthPercent = 0.3f;

        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.HealthCompo.OnApplyDamaged += HandleHitEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.HealthCompo.OnApplyDamaged -= HandleHitEvent;
        }

        public void StartBuff()
        {
            RuntimeManager.PlayOneShot(berserkerSound, _player.transform.position);
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }

        public void EndBuff()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
            _player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            _player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }

        private void HandleHitEvent(float damage)
        {
            Health health = _player.HealthCompo;
            if (health.CurrentHealth <= health.MaxHealth * healthPercent)
            {
                BuffPassiveInfo.ApplyBuffEvent?.Invoke();
                CooldownPassiveInfo.StartCooldownEvent?.Invoke();
            }
        }
    }
}