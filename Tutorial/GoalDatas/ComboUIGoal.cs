using BIS.Events;
using BIS.Manager;
using BIS.UI.Scenes;
using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using System;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class ComboUIGoal : GoalDataElem
    {
        private GameEventChannelSO so;
        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            so = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            Managers.UI.GetSceneUI<TutorialSceneUI>().IsCantOpen = false;
            so.AddListener<ComboEquipEvent>(HandleEquipEvent);
        }

        private void HandleEquipEvent(ComboEquipEvent @event)
        {
            Debug.Log(@event.isSucees);
            goal.HandleIncreaseAmount();
        }

        public override void HandleEndEvent()
        {
        }
    }
}
