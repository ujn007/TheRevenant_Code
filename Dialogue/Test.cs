using UnityEngine;
using BIS.Manager;

namespace KHJ.Dialogue
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private DialogueSO _so;
        private void Awake()
        {
            Managers.UI.ShowPopup<DialoguePopupUI>().ShowText(_so);
        }
    }
}
