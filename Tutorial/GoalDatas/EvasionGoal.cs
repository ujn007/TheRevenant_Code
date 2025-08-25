using PJH.Runtime.Players;

namespace KHJ.Tutorial
{
    public class EvasionGoal : GoalDataElem
    {
        private PlayerHealth playerHealth;
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            enemyGroup.EnableEnemyBT(0, true);

            playerHealth = player.HealthCompo as PlayerHealth;
            playerHealth.OnAvoidingAttack += goal.HandleIncreaseAmount;
        }

        public override void HandleEndEvent()
        {
            playerHealth.OnAvoidingAttack -= goal.HandleIncreaseAmount;
        }
    }
}