//using Main.Runtime.Core.StatSystem;
//using Main.Shared;
//using PJH.Runtime.PlayerPassive;
//using PJH.Runtime.Players;
//using Sirenix.Serialization;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using UnityEngine;

//namespace KHJ.Passive
//{
//    public enum StatType
//    {
//        WalkSpeed,
//        RunSpeed,
//        Power,
//        MaxMomentumGauge,
//        MaxHealth,
//        IncreaseMomentumGuage
//    }

//    public class ModifierStatHandler 
//    {
//        private readonly ModifierStatInfo[] _modifierStatInfo;
//        private readonly Player _player;
//        private readonly Guid _uniqueId;

//        public ModifierStatHandler(ModifierStatInfo[] modifierStatInfo, Player player, Guid guid)
//        {
//            _modifierStatInfo = modifierStatInfo;
//            _player = player;
//            _uniqueId = guid;
//        }

//        public void ModifyStat()
//        {
//            foreach (ModifierStatInfo statInfo in _modifierStatInfo)
//            {
//                _player.GetCompo<PlayerStat>().AddValuePercentModifier(
//                    statInfo.modifierStat, $"{_uniqueId}_{statInfo.modifierStat.statName}", statInfo.modifierValue);
//            }
//        }

//        public StatSO GetStat(StatType stat)
//        {
//            foreach (ModifierStatInfo statInfo in _modifierStatInfo)
//            {
//                if (statInfo.modifierStat.statName == stat.ToString())
//                    return statInfo.modifierStat;
//            }

//            Debug.LogError("스탯이 존재하지 않아요~~");
//            return null;
//        }
//    }
//}
