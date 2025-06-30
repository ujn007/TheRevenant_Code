using BIS.Data;
using Main.Runtime.Agents;
using Main.Runtime.Core.Events;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BadHandPassiveSO", menuName = "SO/Passive/Middle/BadHandPassiveSO")]
    public class BadHandPassiveSO : PassiveSO, IActivePassive, IEffectPoolPassive
    {
        [SerializeField] private CurrencySO _money;
        [SerializeField] private int hitMoney, killMoney;
        [Range(1, 100)][SerializeField] private int hitMoneyPercent;

        [field:SerializeField] public PoolTypeSO PoolType { get; set; }

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
            Agent enemy = info.hitTarget as Agent;
            Vector3 hitPoint = enemy.HealthCompo.GetDamagedInfo.hitPoint;

            if (enemy.HealthCompo.CurrentHealth <= 0)
            {
                _money.AddAmmount(killMoney);
                PlayEffect(PoolType, hitPoint, Quaternion.identity);
                return;
            }

            int rand = Random.Range(0, 100);
            if (rand <= hitMoneyPercent)
            {
                _money.AddAmmount(hitMoney);
                PlayEffect(PoolType, hitPoint, Quaternion.identity);
            }
        }


    }
}
