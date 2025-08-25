using PJH.Runtime.Players;
using System;
using TypeInspector;
using UnityEngine;

namespace KHJ.Tutorial
{
    [CreateAssetMenu(fileName = "GoalDataSO", menuName = "SO/Tutorial/GoalDataSO")]
    public class GoalData : ScriptableObject
    {
        public GoalDataElem goalDataElem;

        public void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup, TypeReference dataType)
        {
            object goalDataElemInstance = Activator.CreateInstance(dataType.Get());
            goalDataElem = goalDataElemInstance as GoalDataElem;
            goalDataElem.Init(goal, player, enemyGroup);
        }

        private void OnDestroy()
        {
            goalDataElem.OnDestroy();
        }
    }
}