using Main.Runtime.Combat;
using PJH.Runtime.Players;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class StrongAttackGoal : GoalDataElem
    {
        private bool isAttack;
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            enemy.GetComponent<Health>().OnApplyDamaged += HandleEnemyHit;
            player.GetCompo<PlayerAttack>().OnAttack += HandleAttack;
        }

        public override void HandleEndEvent()
        {
            enemy.GetComponent<Health>().OnApplyDamaged -= HandleEnemyHit;
            player.GetCompo<PlayerAttack>().OnAttack -= HandleAttack;
        }

        private void HandleAttack() => isAttack = false;

        private void HandleEnemyHit(float v)
        {
            if (isAttack) goal.HandleIncreaseAmount();
        }
    }
}