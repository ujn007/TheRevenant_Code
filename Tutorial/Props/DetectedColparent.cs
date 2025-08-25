using BIS.Manager;
using Main.Runtime.Core.Events;
using System.Collections.Generic;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class DetectedColparent : MonoBehaviour
    {
        public GameEventChannelSO EventSO { get; private set; }

        [SerializeField] private List<DetectedTutorialCollider> colliderList = new();
        private int index = 0;

        private void Awake()
        {
            EventSO = Managers.Resource.Load<GameEventChannelSO>("GameEventChannel");
            EventSO.AddListener<ClearTutorial>(HandleClearTut);
        }
        private void Start()
        {
            colliderList.ForEach(x => x.Initialize(this));
        }
        private void HandleClearTut(ClearTutorial tutorial)
        {
            OpenDoor();
        }

        public void OpenDoor()
        {
            colliderList[index++].OpenDoor();
        }

        public void CloseDoor()
        {
            colliderList[index - 1].CloseDoor();
        }
    }
}
