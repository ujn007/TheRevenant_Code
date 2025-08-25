using BIS.Data;
using BIS.Manager;
using BIS.UI.Scenes;
using KHJ.Dialogue;
using Main.Runtime.Core.Events;
using Main.Runtime.Manager;
using PJH.Runtime.PlayerPassive;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private CurrencySO _money;
        [SerializeField] private List<DialogueSO> dialogueSO;
        [SerializeField] private List<QuestListWrapper> questList = new();
        [Space] [SerializeField] private List<CommandActionPieceSO> _tutorialComboSOs;
        [SerializeField] private List<PassiveSO> _tutorialPassives;
        private InventorySO _inventorySO;

        private bool isNextGoal;
        public int tutCount = 0;
        private int index = 0;
        private bool isEndTutorial = false;

        private void Awake()
        {
            _money = BIS.Manager.Managers.Resource.Load<CurrencySO>("Money");
            _inventorySO = Managers.Resource.Load<InventorySO>("InventorySO");
            eventSO = Managers.Resource.Load<GameEventChannelSO>("GameEventChannel");
            eventSO.AddListener<StartTutorial>(HandleStartTurtorial);
        }

        private void Start()
        {
            Managers.UI.GetSceneUI<TutorialSceneUI>().IsCantOpen = true;
            _money.AddAmmount(1000);
            AddTutorialElement();
        }

        private void OnDestroy()
        {
            eventSO.RemoveListener<StartTutorial>(HandleStartTurtorial);
        }

        private void AddTutorialElement()
        {
            for (int i = 0; i < _tutorialComboSOs.Count; i++)
            {
                CommandActionPieceSO comboSO = ScriptableObject.Instantiate(_tutorialComboSOs[i]);
                comboSO.TryAddPassive(_tutorialPassives[i]);
                _inventorySO.AddElement(comboSO);
            }
        }

        private void HandleStartTurtorial(StartTutorial tutorial)
        {
            if (questList.Count <= index)
            {
                ShowDaialogue(dialogueSO.Count - 1, () => Managers.Scene.LoadScene("Lobby"));
                return;
            }

            isNextGoal = false;
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
            GoalSO.Init(this, eventSO, PlayerManager.Instance.Player as Player, enemyGroup, index, QuestName,
                Description, CurrentAmount, RequiredAmount,DataType, isEnd);

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
                //if (GoalSO.goalData.goalDataElem.GetType() == typeof(ComboMergeGoal))
                //{
                //    AddQuest();
                //    isNextGoal = false;
                //}
                //else
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

        public void ShowDaialogue(int index, Action action)
        {
            Managers.Game.IsTutorialComplete = true;
            DialoguePopupUI ui = Managers.UI.ShowPopup<DialoguePopupUI>();
            ui.ShowText(dialogueSO[index]);
            ui.DialogueFinishEvent += action;
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