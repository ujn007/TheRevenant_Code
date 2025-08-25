using PJH.Runtime.Players;
using System;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class EvasionWhileHittingGoal : GoalDataElem
    {
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            player.GetCompo<PlayerMovement>().OnEvasionWhileHitting += goal.HandleIncreaseAmount; 
        }

        public override void HandleEndEvent()
        {
            player.GetCompo<PlayerMovement>().OnEvasionWhileHitting -= goal.HandleIncreaseAmount; 
        }
    }
}
