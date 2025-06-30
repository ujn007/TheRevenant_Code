using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ
{
    public class VirtualParts : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gradeTxt;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetGradeAndColor(int v, Color color)
        {
            gradeTxt.text = v.ToString();
            image.color = color;
        }
    }
}
