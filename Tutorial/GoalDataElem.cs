using PJH.Runtime.Players;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public abstract class GoalDataElem
    {
        protected Goal goal;
        protected Player player;
        protected TutorialEnemyGroup enemyGroup;
        protected BaseEnemy enemy;

        public virtual void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            this.goal = goal;
            this.player = player;
            this.enemyGroup = enemyGroup;
            this.enemy = enemyGroup.GetEnemy(goal.Index);

            goal.OnEnd += HandleEndEvent;
        }

        public void OnDestroy()
        {
            goal.OnEnd -= HandleEndEvent;
        }

        protected void EnableInputs(bool isEn)
        {
            player.PlayerInput.EnablePlayerInput(isEn);
            player.PlayerInput.EnableUIInput(isEn);
        }

        protected void EnableCursor(bool isEnable)
        {
            Cursor.lockState = isEnable ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isEnable;
        }

        public abstract void HandleEndEvent();
    }
}