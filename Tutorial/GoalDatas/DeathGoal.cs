using PJH.Runtime.Players;

namespace KHJ.Tutorial
{
    public class DeathGoal : GoalDataElem
    {
        TutorialEnemyHealth enemyHealth;

        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            enemyHealth = enemy.GetComponent<TutorialEnemyHealth>();
            enemyHealth.IsCanDeath = true;
            enemyHealth.OnDeath += HandleInc;
        }

        public override void HandleEndEvent()
        {
            enemyHealth.OnDeath -= HandleInc;
        }

        private void HandleInc() => goal.HandleIncreaseAmount();
    }
}
