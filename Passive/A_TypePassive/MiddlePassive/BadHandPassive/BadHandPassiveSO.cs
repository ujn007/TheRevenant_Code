using BIS.Data;
using BIS.Manager;
using FMODUnity;
using KHJ.Shared;
using Main.Runtime.Agents;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BadHandPassiveSO", menuName = "SO/Passive/Middle/BadHandPassiveSO")]
    public class BadHandPassiveSO : PassiveSO, IActivePassive, IEffectPoolPassive
    {
        public EventReference sound;
    private CurrencySO _money;
        [SerializeField] private int hitMoney, killMoney;
        [Range(1, 100)][SerializeField] private int hitMoneyPercent;

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
            _money = Managers.Resource.Load<CurrencySO>("Money");
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
                RuntimeManager.PlayOneShot(sound, hitPoint);
                PlayEffect(PoolType, hitPoint, Quaternion.identity);
            }
        }


    }
}
