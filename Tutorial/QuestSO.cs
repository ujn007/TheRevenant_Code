using TypeInspector;
using UnityEngine;

namespace KHJ.Tutorial
{
    [CreateAssetMenu(fileName = "QuestSO", menuName = "SO/Quest")]
    public class QuestSO : ScriptableObject
    {
        public string QuestName;
        public string Description;
        public int RequiredAmount;
        public TypeReference dataType;
    }
}
