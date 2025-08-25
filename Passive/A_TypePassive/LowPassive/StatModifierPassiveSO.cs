using KHJ.Shared;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(menuName = "SO/Passive/AddStatModifierPassive")]
    public class StatModifierPassiveSO : PassiveSO, IModifierStatPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; } = new();

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _player = player as Player;
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}