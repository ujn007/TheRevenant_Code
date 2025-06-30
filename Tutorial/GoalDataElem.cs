using PJH.Runtime.Players;
using System;
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

        public abstract void HandleEndEvent();
    }
}