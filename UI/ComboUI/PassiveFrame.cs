using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class PassiveFrame : MonoBehaviour
    {
        [SerializeField] private Image _passiveImage;
        [SerializeField] private Image _coolDownImage;
        [SerializeField] private TextMeshProUGUI _coolDownText;

        public void ShowPassiveImage(Sprite sprite)
        {
            _passiveImage.sprite = sprite;
        }

        public void ShowCoolDownImage(Sprite sprite) 
        {
            _passiveImage.sprite = sprite;
        }

        public void ShowCoolDownText(float value)
        {
            _coolDownText.text = value.ToString();
        }
    }
}
