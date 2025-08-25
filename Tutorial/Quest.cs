using TypeInspector;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class Quest : MonoBehaviour
    {
        public Goal GoalSO;
        public string QuestName { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int CurrentAmount { get; set; }
        public int RequiredAmount { get; set; }
        public TypeReference DataType { get; set; }

        public virtual void Update()
        {
            CheckGoals();
        }

        public void CheckGoals()
        {
            Completed = GoalSO.Completed;
            bool isCanGiveReward = Completed && Time.timeScale > 0;
            if (isCanGiveReward) GiveReward();
        }

        public virtual void GiveReward()
        {
        }
    }
}
