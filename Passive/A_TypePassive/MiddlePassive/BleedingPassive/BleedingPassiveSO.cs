using DG.Tweening;
using Main.Runtime.Agents;
using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BleedingPassiveSO", menuName = "SO/Passive/Middle/BleedingPassiveSO")]
    public class BleedingPassiveSO : PassiveSO, ICooldownPassive
    {
        public float dotDealDuration;
        public float dotDealDamage;
        [field: SerializeField, OdinSerialize] public CooldownPassiveInfo CooldownPassiveInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.OnHitTarget += HandleHitEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.OnHitTarget -= HandleHitEvent;
        }

        private void HandleHitEvent(HitInfo info)
        {
            Agent target = info.hitTarget as Agent;
            target.HealthCompo.ailmentStat.ApplyAilments(Ailment.Dot, dotDealDuration, dotDealDamage);
        }
    }
}