using Main.Runtime.Combat;
using Main.Runtime.Combat.Core;
using PJH.Runtime.Players;
using System;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class AttackGoalData : GoalDataElem
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

        private void HandleAttack() => isAttack = true;

        private void HandleEnemyHit(float damage)
        {
            if (isAttack) goal.HandleIncreaseAmount();
        }
    }
}