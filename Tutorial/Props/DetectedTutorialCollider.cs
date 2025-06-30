using Main.Runtime.Core.Events;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class DetectedTutorialCollider : MonoBehaviour
    {
        [SerializeField] private SlideDoor slideDoor;
        private DetectedColparent colPar;

        public void Initialize(DetectedColparent par)
        {
            colPar = par;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                colPar.CloseDoor();
                StartTutorial evt = GameEvents.StartTutorial;
                colPar.EventSO.RaiseEvent(evt);
            }
        }

        public void OpenDoor() => slideDoor.OpenDoor();
        public void CloseDoor() => slideDoor.CloseDoor();
    }
}
