using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class CurrentComboIcon : MonoBehaviour
    {
        public RectTransform rect => transform as RectTransform;
        [SerializeField] private Image fadeImage;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image icon;
        [SerializeField] private float fadeValue;

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public void SetText(int index)
        {
            text.text = index.ToString();
        }

        public void SetDOFade(bool isActive, float duration)
        {
            _isActive = isActive;
            float value = isActive ? 0 : fadeValue;
            fadeImage.DOFade(value, duration).SetLink(gameObject);
        }
    }
}