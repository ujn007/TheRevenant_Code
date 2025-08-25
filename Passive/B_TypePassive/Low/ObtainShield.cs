using System;
using BIS.Manager;
using FMODUnity;
using KHJ.Shared;
using Main.Core;
using Main.Runtime.Core.Events;
using Main.Shared;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KHJ.Passive
{
    [CreateAssetMenu(fileName = "ObtainShield", menuName = "SO/Passive/Low/ObtainShield")]
    public class ObtainShield : PassiveSO, IEffectPoolPassive, IModifierStatPassive
    {
        public EventReference sound;
        private GameEventChannelSO _eventSO;
        [SerializeField] private int _percent;
        [field: SerializeField, OdinSerialize] public ModifierStatInfo ModifierStatInfo { get; set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }

        public override void EquipPiece(IPlayer player)
        {
            base.EquipPiece(player);
            _eventSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");

            _eventSO.AddListener<StartWave>(HandleStartWaveEvent);
        }

        public override void UnEquipPiece()
        {
            base.UnEquipPiece();
            _eventSO.RemoveListener<StartWave>(HandleStartWaveEvent);
        }

        private void HandleStartWaveEvent(StartWave wave)
        {
            int rand = Random.Range(1, 101);
            if (rand <= _percent)
            {
                ModifierStatInfo.AddModifierEvent?.Invoke(ModifierStatInfo);
                _player.GetCompo<PlayerEffect>().PlayEffectAttachedToBody(PoolType, HumanBodyBones.Spine);
            }
        }
    }
}