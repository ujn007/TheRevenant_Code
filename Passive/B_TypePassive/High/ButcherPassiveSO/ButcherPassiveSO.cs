using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "ButcherPassiveSO", menuName = "SO/Passive/High/ButcherPassiveSO")]
    public class ButcherPassiveSO : PassiveSO, IModifierStatPassive, IEffectPoolPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
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
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.LeftHand);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.RightHand);
        }
    }
}
