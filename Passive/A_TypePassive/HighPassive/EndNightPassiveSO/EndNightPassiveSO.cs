using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "EndNightPassiveSO", menuName = "SO/Passive/High/EndNightPassiveSO")]
    public class EndNightPassiveSO : PassiveSO, ICooldownPassive, IModifierStatPassive, IBuffPassive, IEffectPoolPassive
    {
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        private PlayerHealth _playerHealth;

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _playerHealth = _player.HealthCompo as PlayerHealth;
            _playerHealth.OnAvoidingAttack += HandleAvoidEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _playerHealth.OnAvoidingAttack -= HandleAvoidEvent;
        }

        private void HandleAvoidEvent()
        {
            Debug.Log("회피 시작작작작");
            BuffPassiveInfo.ApplyBuffEvent?.Invoke();
        }

        public void StartBuff()
        {
            Debug.Log("버프 시작작작작");
            _player.HealthCompo.IsInvincibility = true;
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            CooldownPassiveInfo.StartCooldownEvent?.Invoke();
        }

        public void EndBuff()
        {
            _player.HealthCompo.IsInvincibility = false;
            _player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
