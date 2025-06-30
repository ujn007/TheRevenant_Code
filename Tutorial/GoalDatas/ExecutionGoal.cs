using Main.Runtime.Combat;
using PJH.Runtime.Players;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class ExecutionGoal : GoalDataElem
    {
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            enemyGroup.EnableEnemyBT(enemy, true);
            enemy.GetComponentInChildren<TutorialMomentumGauge>().IsEx = true;
            enemy.GetComponent<Health>().OnDeath += () => 
            {
                goal.HandleIncreaseAmount();
            };
        }

        public override void HandleEndEvent()
        {
            enemy.GetComponent<Health>().OnDeath -= goal.HandleIncreaseAmount;
        }
    }
}