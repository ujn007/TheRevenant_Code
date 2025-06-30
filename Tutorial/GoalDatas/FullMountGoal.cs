using Animancer;
using PJH.Runtime.Players;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class FullMountGoal : GoalDataElem
    {
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            player.GetCompo<PlayerFullMount>().OnFullMount += Handle;
        }

        public override void HandleEndEvent()
        {
            player.GetCompo<PlayerFullMount>().OnFullMount -= Handle;
        }

        private void Handle(ITransition v) => goal.HandleIncreaseAmount();
    }
}