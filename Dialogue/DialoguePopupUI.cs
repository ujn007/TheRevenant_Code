using BIS.Manager;
using BIS.UI.Popup;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.Dialogue
{
    public class DialoguePopupUI : PopupUI
    {
        [SerializeField] private Transform _buttonGroup;
        private List<(Button, TMP_Text)> _choiseButtonList = new();
        private DialogueSO _currentData;
        public event Action DialogueFinishEvent;

        private enum Texts
        {
            NameText,
            ContentText,
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            for (int i = 0; i < _buttonGroup.childCount; i++)
            {
                Transform btnTrm = _buttonGroup.GetChild(i);
                Button button = btnTrm.GetComponent<Button>();
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

                int index = i;
                button.onClick.AddListener(() => HandleChoiseBtn(index));
                button.gameObject.SetActive(false);
                _choiseButtonList.Add((button, buttonText));
            }

            return true;
        }
        public void ShowText(DialogueSO data, bool isFinishMove = false, bool isDontMove = true)
        {
            print("2222");
            _currentData = data;
            if (isDontMove == true)
                _inputSO.EnablePlayerInput(false);

            BindTexts(typeof(Texts));

            StartCoroutine(ShowTextCoroutine(_currentData, isFinishMove));
        }

        private IEnumerator ShowTextCoroutine(DialogueSO data, bool isFinishMove = false)
        {
            TMP_Text text = GetText((int)Texts.ContentText);
            int dataLineLength = data.DialogueLines.Count;
            bool isChoiceDialogue = data.isDialogueChoise;

            var wait = new WaitForSeconds(0.015f);

            for (int i = 0; i < dataLineLength; ++i)
            {
                GetText((int)Texts.NameText).text = data.DialogueLines[i].speaker;

                string line = data.DialogueLines[i].contents;
                text.text = "";

                for (int j = 0; j < line.Length; ++j)
                {
                    text.text += line[j];
                    yield return wait;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        text.text = line;
                        break; // Move Next Line
                    }
                }

                if (i + 1 == dataLineLength && isChoiceDialogue) break;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space));
                data.DialogueLines[i].speakEvent?.Invoke();
            }

            print(isChoiceDialogue);
            if (isChoiceDialogue)
            {
                for (int i = 0; i < data.dialogueChoiseSOList.Count; i++)
                {
                    _choiseButtonList[i].Item1.gameObject.SetActive(true);
                    _choiseButtonList[i].Item2.text = data.dialogueChoiseSOList[i].contents;
                }
            }
            else
            {
                data.DialogueFinishEvent?.Invoke();
                this.DialogueFinishEvent?.Invoke();
                this.DialogueFinishEvent = null;
                if (isFinishMove == true)
                    _inputSO.EnablePlayerInput(true);
                Managers.UI.ClosePopupUI(this);
            }
        }
        private void HandleChoiseBtn(int index)
        {
            DialogueSO nextDialogue = _currentData.dialogueChoiseSOList[index].dialogueSO;

            for (int i = 0; i < _currentData.dialogueChoiseSOList.Count; i++)   
                _choiseButtonList[i].Item1.gameObject.SetActive(false);

            _inputSO.EnablePlayerInput(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(ShowTextCoroutine(nextDialogue));
        }
    }
}