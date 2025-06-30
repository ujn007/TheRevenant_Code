using DG.Tweening;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class SlideDoor : MonoBehaviour
    {
        [SerializeField] private float minYPos, maxYPos;
        [SerializeField] private float duration;

        public void OpenDoor()
        {
            transform.DOLocalMoveY(maxYPos, duration).SetEase(Ease.Linear);
        }

        public void CloseDoor()
        {
            transform.DOLocalMoveY(minYPos, duration).SetEase(Ease.Linear);
        }
    }
}
