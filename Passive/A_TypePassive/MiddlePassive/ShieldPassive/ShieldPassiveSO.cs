using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "ShieldPassiveSO", menuName = "SO/Passive/Middle/ShieldPassiveSO")]
    public class ShieldPassiveSO : PassiveSO, IModifierStatPassive, IActivePassive, IEffectPoolPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        [SerializeField] private int _percent;

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
            if (_player.HealthCompo.CurrentShield > 0) return;

            int rand = Random.Range(1, 101);
            if (rand <= _percent)
            {
                ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
                _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            }
        }
    }
}
