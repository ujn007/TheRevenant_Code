using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using System;
using TypeInspector;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{

    [CreateAssetMenu(fileName = "GoalSO", menuName = "SO/Tutorial/GoalSO")]
    public class Goal : ScriptableObject
    {
        public event Action OnEnd;

        public GameEventChannelSO EventSO { get; set; }
        public GoalData goalData;
        public Player Player { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int CurrentAmount { get; set; }
        public int RequiredAmount { get; set; }
        public bool IsEnd { get; set; }
        public int Index { get; set; }

        public void Init(GameEventChannelSO eventSO, Player player, TutorialEnemyGroup enemyGroup, int index, string name, string description,
            int currentAmount, int requiredAmount, TypeReference dataType, bool isEnd)
        {
            Player = player;
            EventSO = eventSO;
            Name = name;
            Description = description;
            Completed = false;
            CurrentAmount = currentAmount;
            RequiredAmount = requiredAmount;
            IsEnd = isEnd;
            Index = index;
            goalData.Init(this, player, enemyGroup, dataType);

            UpdateValueToUI(true);
        }
        public void End() { }
        public void Update() { }

        public void Evaluate()
        {
            bool v = CurrentAmount >= RequiredAmount;
            if (v) Complete();
        }

        public void Complete()
        {
            Completed = true;
            OnEnd?.Invoke();
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
