using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KHJ.Tutorial
{
    public class AmountUI : MonoBehaviour
    {
        [ColorUsage(true)] public Color color;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image screenImage;
        [SerializeField] private UnityEvent _enableEvent;

        public void Start()
        {
            fillImage.color = color;
            EnableImage(false);
        }

        public void EnableImage(bool isEnable)
        {
            if (isEnable == true)
                _enableEvent?.Invoke();
            fillImage.enabled = isEnable;
        }
        public void EnableScreenImage(bool v)
        {
            screenImage.enabled = v;
            screenImage.DOFade(0.85f, 0.2f).startValue = Vector4.zero;
        }
    }
}
