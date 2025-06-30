using Main.Runtime.Core.Events;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "ObtainShield", menuName = "SO/Passive/Active/Low/ObtainShield")]
    public class ObtainShield : PassiveSO, IBuffPassive, IEffectPoolPassive, IModifierStatPassive
    {
        [SerializeField] private GameEventChannelSO _eventSO;
        [SerializeField] private int _addShieldPercent;
        [field: SerializeField, OdinSerialize]  public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField, OdinSerialize] public BuffPassiveInfo BuffPassiveInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _eventSO.AddListener<StartWave>(HandleStartWaveEvent);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _eventSO.RemoveListener<StartWave>(HandleStartWaveEvent);
        }

        private void HandleStartWaveEvent(StartWave wave)
        {
            BuffPassiveInfo.ApplyBuffEvent?.Invoke();
        }

        public void StartBuff()
        {
            ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
            _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
        }

        public void EndBuff()
        {
        }
    }
}
