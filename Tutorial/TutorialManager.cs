using BIS.Data;
using BIS.Manager;
using BIS.UI.Popup;
using BIS.UI.Scenes;
using KHJ.Dialogue;
using Main.Runtime.Core.Events;
using Main.Runtime.Manager;
using Opsive.BehaviorDesigner.Runtime;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    [System.Serializable]
    public class QuestListWrapper
    {
        public List<QuestSO> quests;
    }

    public class TutorialManager : Quest
    {
        private GameEventChannelSO eventSO;
        [SerializeField] private TutorialEnemyGroup enemyGroup;
        [SerializeField] private DialogueSO dialogueSO;
        [SerializeField] private List<QuestListWrapper> questList = new();

        private bool isNextGoal;
        public int tutCount = 0;
        private int index = 0;
        private DialoguePopupUI _dialogueUI;
        private bool isEndTutorial = false;

        private void Awake()
        {
            eventSO = Managers.Resource.Load<GameEventChannelSO>("GameEventChannel");
            eventSO.AddListener<StartTutorial>(HandleStartTurtorial);
        }

        private void HandleStartTurtorial(StartTutorial tutorial)
        {
            if (questList.Count <= index)
            {
                Managers.Game.IsTutorialComplete = true;
                DialoguePopupUI ui = Managers.UI.ShowPopup<DialoguePopupUI>();
                ui.ShowText(dialogueSO);
                ui.DialogueFinishEvent += () => SceneControlManager.LoadScene("Lobby");
                return;
            }

            isNextGoal = false;
            AddQuest();
        }

        private void Start()
        {
            //Managers.UI.GetSceneUI<TutorialSceneUI>().IsCantOpen = false;
            AddQuest();
        }

        public override void Update()
        {
            base.Update();
            GoalSO.Update();
        }

        public void AddQuest()
        {
            bool isEnd = questList[index].quests.Count <= tutCount + 1;

            GoalInfo();
            GoalSO.Init(eventSO, PlayerManager.Instance.Player as Player, enemyGroup,index, QuestName,
                Description, CurrentAmount, RequiredAmount, DataType, isEnd);

            tutCount++;
        }

        public override void GiveReward()
        {
            base.GiveReward();
            if (isEndTutorial) return;
            GoalSO.End();

            if (questList.Count <= index)
                isEndTutorial = true;

            if (!isNextGoal)
            {
                isNextGoal = true;
                StartCoroutine(WaitAndAddQuest());
            }
        }

        private IEnumerator WaitAndAddQuest()
        {
            yield return new WaitForSeconds(2f);
            if (questList[index].quests.Count <= tutCount)
            {
                index++;
                tutCount = 0;
                ClearTutorial evt = GameEvents.ClearTutorial;
                eventSO.RaiseEvent(evt);
            }
            else
            {
                AddQuest();
                isNextGoal = false;
            }
        }

        private void GoalInfo()
        {
            QuestName = questList[index].quests[tutCount].QuestName;
            Description = questList[index].quests[tutCount].Description;
            RequiredAmount = questList[index].quests[tutCount].RequiredAmount;
            DataType = questList[index].quests[tutCount].dataType;

        }
    }
}