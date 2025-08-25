using KHJ.Shared;
using Main.Runtime.Core.Events;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "LoneFocusPassiveSO", menuName = "SO/Passive/Middle/LoneFocusPassiveSO")]
    public class LoneFocusPassiveSO : PassiveSO, IModifierStatPassive
    {
        [SerializeField] private int _minEnemyCount;

        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _gameEventChannel.AddListener<ChangeCurrentEnemy>(HandleEnemyCountEvent);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _gameEventChannel.RemoveListener<ChangeCurrentEnemy>(HandleEnemyCountEvent);
        }

        private void HandleEnemyCountEvent(ChangeCurrentEnemy enemy)
        {
            int count = enemy.enemyCount;

            if (count <= _minEnemyCount)
                ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            else
                ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
