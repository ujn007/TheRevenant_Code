using System;
using System.Collections.Generic;
using UnityEngine;

namespace KHJ.Dialogue
{
    [CreateAssetMenu(menuName = "SO/KHJ/Dialogue/DialogueData")]
    public class DialogueSO : ScriptableObject
    {
        public Action DialogueFinishEvent;
        public bool isDialogueChoise;
        public bool isRandomSelect;
        [SerializeField] private List<DialogueData> _dialogueLines; public List<DialogueData> DialogueLines { get { return _dialogueLines; } }
        public List<DialogueChoiseSO> dialogueChoiseSOList;
        // public List<>

        private void OnValidate()
        {
            if (isDialogueChoise == true && isRandomSelect == true)
            {
                Debug.LogError("You cannot use both the isDialogueChoice and isRandomSelect features at the same time. Please choose only one.");
                isDialogueChoise = false;
                isRandomSelect = false;
            }
        }
    }
}
