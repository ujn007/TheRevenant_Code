using Main.Runtime.Combat;
using PJH.Runtime.Players;

namespace KHJ.Tutorial
{
    public class AttackGoalData : GoalDataElem
    {
        private bool isAttack;
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);

            goal.ShowDialogue(0, () =>
            {
                EnableInputs(true);
                enemy.GetComponent<Health>().OnApplyDamaged += HandleEnemyHit;
                player.GetCompo<PlayerAttack>().OnAttack += HandleAttack;
            });
        }

        public override void HandleEndEvent()
        {
            enemy.GetComponent<Health>().OnApplyDamaged -= HandleEnemyHit;
            player.GetCompo<PlayerAttack>().OnAttack -= HandleAttack;
        }

        private void HandleAttack() => isAttack = true;

        private void HandleEnemyHit(float damage)
        {
            if (isAttack) goal.HandleIncreaseAmount();
        }
    }
}