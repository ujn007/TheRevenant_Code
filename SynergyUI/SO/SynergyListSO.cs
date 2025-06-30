using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace KHJ.SO
{
    [CreateAssetMenu(fileName = "SynergyListSO", menuName = "SO/KHJ/SynergyListSO")]
    public class SynergyListSO : ScriptableObject
    {
        public List<SynergySO> synergyList = new List<SynergySO>();

        private readonly string soPath = "Assets/00Work/KHJ/08.SO/Synergy";

        public SynergySO GetSynergySO(string id) => synergyList.FirstOrDefault(synergy => synergy.ItemID == id);

        [Button("Update Slots", ButtonSizes.Medium)]
        private void GetSynergyList()
        {
#if UNITY_EDITOR
            synergyList.Clear();
            string[] guids = AssetDatabase.FindAssets("t:SynergySO", new[] { soPath });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SynergySO so = AssetDatabase.LoadAssetAtPath<SynergySO>(path);
                if (so != null)
                    synergyList.Add(so);
            }
#endif
        }
    }
}
