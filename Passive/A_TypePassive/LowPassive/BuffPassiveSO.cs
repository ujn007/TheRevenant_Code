using Main.Runtime.Core.StatSystem;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "BuffPassiveSO", menuName = "SO/Passive/Buff/BuffPassiveSO")]
    public class BuffPassiveSO : PassiveSO, IModifierStatPassive
    {
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);

            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            ModifierStatInfo.RemoveModifierEvent?.Invoke(ModifierStatInfo);
        }
    }
}
