using FMODUnity;
using KHJ.Shared;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "ButcherPassiveSO", menuName = "SO/Passive/High/ButcherPassiveSO")]
    public class ButcherPassiveSO : PassiveSO, IModifierStatPassive, IEffectPoolPassive, IBuffPassive, ICooldownPassive
    {
        public EventReference sound;
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.GetCompo<PlayerEnemyFinisher>().OnFinisherEnd += HandleFinisherEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.GetCompo<PlayerEnemyFinisher>().OnFinisherEnd -= HandleFinisherEvent;
        }

        private void HandleFinisherEvent()
        {
            BuffPassiveInfo.ApplyBuffEvent?.Invoke();
            CooldownPassiveInfo?.StartCooldownEvent?.Invoke();
        }

        public void StartBuff()
        {
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            RuntimeManager.PlayOneShot(sound);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }

        public void EndBuff()
        {
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
            _player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            _player.GetCompo<PlayerEffect>().StopEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }
    }
}
