using KHJ.Enum;
using System.Collections.Generic;
using Main.Runtime.Core.StatSystem;
using UnityEngine;
using Sirenix.OdinInspector;
using BIS.Data;

namespace KHJ.SO
{
    [CreateAssetMenu(fileName = "SynergySO", menuName = "SO/Synergy/SynergySO")]
    public class SynergySO : ItemSO
    {
        public int grade;

        [OnValueChanged("SetColorToType"), Delayed]
        public SynergyType synergyType;

        public StatSO increasePlayerStat;
        public float increasePlayerStatValue;
        public List<Vector2Int> spaces = new List<Vector2Int> { Vector2Int.zero };
        public Color typeColor;

        private void SetColorToType() => typeColor = SetColor(synergyType);

        public Color SetColor(SynergyType type) => synergyColors[type];

        private readonly Dictionary<SynergyType, Color> synergyColors = new Dictionary<SynergyType, Color>
    {
        { SynergyType.Warrior, Color.red },
        { SynergyType.Guardian, Color.blue }, 
        { SynergyType.Stun, new Color(0.5f, 0, 0.5f) },
        { SynergyType.Health, Color.green },
        { SynergyType.Money, Color.yellow },
        { SynergyType.Empty, Color.white }
    };


    }
}
