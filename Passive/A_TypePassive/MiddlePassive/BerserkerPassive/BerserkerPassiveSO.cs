using System;
using Main.Runtime.Combat;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BerserkerPassiveSO", menuName = "SO/Passive/Middle/BerserkerPassiveSO")]
    public class BerserkerPassiveSO : PassiveSO, IBuffPassive, IActivePassive,
        ICooldownPassive, IModifierStatPassive, IEffectPoolPassive
    {
        [SerializeField] private float healthPercent = 0.3f;

        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField]  public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            BuffPassiveInfo.ApplyBuffEvent?.Invoke();
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }

        public void StartBuff()
        {
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
            Health health = _player.HealthCompo;
            if (health.CurrentHealth <= health.MaxHealth * healthPercent)
            {
                BuffPassiveInfo.ApplyBuffEvent?.Invoke();
                CooldownPassiveInfo.StartCooldownEvent?.Invoke();
            }
        }
    }
}