using BIS.Events;
using BIS.Manager;
using BIS.UI.Scenes;
using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class ComboMergeGoal : GoalDataElem
    {
        private GameEventChannelSO so;
        private bool isAlreadySpeak = false;
        private bool isMergeComplete = false;

        public override void Init(Goal goal, Player player, TutorialEnemyGroup enemyGroup)
        {
            base.Init(goal, player, enemyGroup);

            EnableInputs(false);
            goal.ShowDialogue(1, () =>
            {
                EnableInputs(true);
                so = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
                Managers.UI.GetSceneUI<TutorialSceneUI>().IsCantOpen = false;
                so.AddListener<ComboSyntheticEvent>(HandleMergeEvent);
                so.AddListener<ComboSyntheticOpenEvent>(HandleMergeOpenEvent);
            });
        }

        public override void HandleEndEvent()
        {
            so.RemoveListener<ComboSyntheticEvent>(HandleMergeEvent);
            so.RemoveListener<ComboSyntheticOpenEvent>(HandleMergeOpenEvent);
        }

        public void HandleMergeOpenEvent(ComboSyntheticOpenEvent @event)
        {
            if (isAlreadySpeak) return;
            isAlreadySpeak = true;

            EnableInputs(false);
            EnableCursor(false);

            goal.ShowDialogue(2, () =>
            {
                EnableInputs(true);
                EnableCursor(true);
            });
        }

        private void HandleMergeEvent(ComboSyntheticEvent @event)
        {
            goal.HandleIncreaseAmount();
        }
    }
}
