using UnityEngine;

namespace KHJ.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueChoiseSO", menuName = "SO/KHJ/Dialogue/DialogueChoiseSO")]
    public class DialogueChoiseSO : ScriptableObject
    {
        public string contents;
        public DialogueSO dialogueSO;
    }
}
