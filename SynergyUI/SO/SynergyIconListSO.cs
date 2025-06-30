using System.Collections.Generic;
using UnityEngine;
using System;
using KHJ.Enum;
namespace KHJ.SO
{
    [Serializable]
    public struct SynergyIconData
    {
        public SynergyType synergyType;
        public Sprite icon;
    }
    [CreateAssetMenu(menuName = "BIS/SO/Data/SynergyIconList")]
    public class SynergyIconListSO : ScriptableObject
    {
        [SerializeField] private List<SynergyIconData> _synergyIconDatas; public List<SynergyIconData> SynergyIconDatas { get { return _synergyIconDatas; } }
    }
}
