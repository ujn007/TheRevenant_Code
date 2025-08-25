using KHJ.Shared;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "KeenSensePassiveSO", menuName = "SO/Passive/High/KeenSensePassiveSO")]
    public class KeenSensePassiveSO : PassiveSO, IModifierStatPassive, ICooldownPassive, IBuffPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }

        private PlayerHealth playerHealth;

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            playerHealth = _player.HealthCompo as PlayerHealth;
            playerHealth.OnAvoidingAttack += HandleAvoidEvent;
            _player.OnHitTarget += HandleHitEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            playerHealth.OnAvoidingAttack -= HandleAvoidEvent;
            _player.OnHitTarget -= HandleHitEvent;
        }

        private void HandleAvoidEvent()
        {
            BuffPassiveInfo.ApplyBuffEvent?.Invoke();
        }

        private void HandleHitEvent(HitInfo info)
        {
            EndPassive();
        }

        public void StartBuff()
        {
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            //_player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            //_player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }
        
        public void EndBuff()
        {
            EndPassive();
        }

        private void EndPassive()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
            CooldownPassiveInfo.StartCooldownEvent?.Invoke();

            //_player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            //_player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }
    }
}
