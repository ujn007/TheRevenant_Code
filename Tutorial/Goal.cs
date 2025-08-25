using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Threading.Tasks;
using TypeInspector;
using UnityEngine;

namespace KHJ.Tutorial
{
    [CreateAssetMenu(fileName = "GoalSO", menuName = "SO/Tutorial/GoalSO")]
    public class Goal : ScriptableObject
    {
        public event Action OnEnd;

        public GameEventChannelSO EventSO { get; set; }
        public GoalData goalData;
        public Player Player { get; private set; }
        public TutorialManager TutorialManager { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int CurrentAmount { get; set; }
        public int RequiredAmount { get; set; }
        public bool IsEnd { get; set; }
        public int Index { get; set; }

        public void Init(TutorialManager tutorialManager, GameEventChannelSO eventSO, Player player,
            TutorialEnemyGroup enemyGroup, int index, string name, string description,
            int currentAmount, int requiredAmount, TypeReference dataType, bool isEnd)
        {
            Player = player;
            EventSO = eventSO;
            TutorialManager = tutorialManager;
            Name = name;
            Description = description;
            Completed = false;
            CurrentAmount = currentAmount;
            RequiredAmount = requiredAmount;
            IsEnd = isEnd;
            Index = index;
            goalData.Init(this, player, enemyGroup,dataType);

            Debug.Log($"진짜 이거 왜안돼 : {Name} , {IsEnd}");
            UpdateValueToUI(true);
        }

        public void End()
        {
        }

        public void Update()
        {
        }

        public void Evaluate()
        {
            //await Task.Delay(300);
            bool v = CurrentAmount >= RequiredAmount;
            if (v) Complete();
        }

        public void Complete()
        {
            Debug.Log(
                "dofgnoiughiougheoighouigroigrouhigrdhouirgohugrohgrogohugrhourpoiuuhygerwp;oiuhgerpoz;aGIH;oierzgh");
            Completed = true;
            OnEnd?.Invoke();
        }

        public void ShowDialogue(int index, Action action)
        {
            TutorialManager.ShowDaialogue(index, action);
        }

        /// <summary>
        /// 목표 행동
        /// </summary>
        public void HandleIncreaseAmount()
        {
            CurrentAmount++;
            Evaluate();
            UpdateValueToUI(isEnd: IsEnd);
        }

        public virtual void UpdateValueToUI(bool isStartQuest = false, bool isEnd = false)
        {
            TutorialInfo tutorialInfo = GameEvents.TutorialInfo;
            tutorialInfo.IsEndQuest = isEnd;
            tutorialInfo.IsStartQuest = isStartQuest;
            tutorialInfo.QuestName = Name;
            tutorialInfo.Description = Description;
            tutorialInfo.RequiredAmount = RequiredAmount;
            tutorialInfo.CurrentAmount = CurrentAmount;
            EventSO.RaiseEvent(tutorialInfo);
        }
    }
}