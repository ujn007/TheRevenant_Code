using Main.Shared;
using PJH.Runtime.PlayerPassive;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "MadnessRhythmPassiveSO", menuName = "SO/Passive/High/MadnessRhythmPassiveSO")]
    public class MadnessRhythmPassiveSO : PassiveSO, IModifierStatPassive
    {
        [SerializeField] private int _hitStack;
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player.OnHitTarget += HandleHitEvent;
            _player.HealthCompo.OnApplyDamaged += HandleDamagedEvent;
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _player.OnHitTarget -= HandleHitEvent;
            _player.HealthCompo.OnApplyDamaged -= HandleDamagedEvent;
        }

        private void HandleHitEvent(HitInfo info)
        {
            ++_hitStack;
            ChangeStatValue(_hitStack);
        }

        private void HandleDamagedEvent(float obj)
        {
            _hitStack = 0;
            ChangeStatValue(_hitStack); 
        }

        private void ChangeStatValue(int value)
        {
            for (int i = 0; i < ModifierStatInfo.ModifierStats.Length; i++)
            {
                ModifierStatInfo.ModifierStats[i].modifierValue = value;
            }

            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
