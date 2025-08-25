using PJH.Runtime.Players;

namespace KHJ.Tutorial
{
    public class ParryingGoal : GoalDataElem
    {
        private PlayerHealth playerHealth;
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            playerHealth = player.HealthCompo as PlayerHealth;
            playerHealth.OnParrying += goal.HandleIncreaseAmount;
        }

        public override void HandleEndEvent()
        {
            playerHealth.OnParrying -= goal.HandleIncreaseAmount;
        }
    }
}