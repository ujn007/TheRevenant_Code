using BIS.Events;
using BIS.Manager;
using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class ComboEquipGoal : GoalDataElem
    {
        private GameEventChannelSO so;
        private bool isAlreadySpeak;

        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);
            so = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            so.AddListener<ComboEquipEvent>(HandleEquipEvent);
            so.AddListener<ComboSettingOpenEvent>(HandleEquipOpenEvent);
        }

        public override void HandleEndEvent()
        {
            so.RemoveListener<ComboEquipEvent>(HandleEquipEvent);
            so.RemoveListener<ComboSettingOpenEvent>(HandleEquipOpenEvent);
        }

        public void HandleEquipOpenEvent(ComboSettingOpenEvent @event)
        {
            if (isAlreadySpeak) return;
            isAlreadySpeak = true;

            EnableInputs(false);
            EnableCursor(false);

            goal.ShowDialogue(3, () =>
            {
                EnableInputs(true);
                EnableCursor(true);
            });
        }

        private void HandleEquipEvent(ComboEquipEvent @event)
        {
            goal.HandleIncreaseAmount();
        }
    }
}
