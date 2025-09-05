using BIS.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using Main.Core;
using Main.Runtime.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = Main.Core.Debug;
using Managers = BIS.Manager.Managers;

namespace KHJ.Dialogue
{
    public class DialoguePopupUI : PopupUI
    {
        public event Action DialogueFinishEvent;
        [SerializeField] private Transform _buttonGroup;
        private List<(Button, TMP_Text)> _choiseButtonList = new();
        private DialogueSO _currentData;


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
                button.onClick.AddListener(() => HandleChoiceBtn(index));
                button.gameObject.SetActive(false);
                _choiseButtonList.Add((button, buttonText));
            }

            return true;
        }

        public void ShowText(DialogueSO data, bool isFinishMove = false, bool isDontMove = true)
        {
            _currentData = data;
            if (isDontMove == true)
            {
                var evt = GameEvents.EnableCameraMovement;
                evt.enableCameraMovmement = false;
                _gameEventChannel.RaiseEvent(evt);
                _inputSO.EnablePlayerInput(false);
            }

            BindTexts(typeof(Texts));

            StartCoroutine(ShowTextCoroutine(_currentData, isFinishMove));
        }

        private IEnumerator ShowTextCoroutine(DialogueSO data, bool isFinishMove = false)
        {
            TMP_Text text = GetText((int)Texts.ContentText);
            int dataLineLength = data.DialogueLines.Count;
            bool isChoiceDialogue = data.isDialogueChoise;

            var wait = new WaitForSecondsRealtime(0.015f);

            for (int i = 0; i < dataLineLength; ++i)
            {
                GetText((int)Texts.NameText).text = data.DialogueLines[i].speaker;
                string line = data.DialogueLines[i].contents;
                text.text = "";

                var sb = new System.Text.StringBuilder(line.Length);

                for (int j = 0; j < line.Length; ++j)
                {
                    sb.Append(line[j]);
                    text.text = sb.ToString();
                    yield return wait;

                    Main.Runtime.Manager.Managers.FMODManager.PlayTypingSound();

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        text.text = line;
                        break;
                    }
                }

                if (i + 1 == dataLineLength && isChoiceDialogue) break;

                yield return new WaitForSecondsRealtime(0.5f);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space));

                Main.Runtime.Manager.Managers.FMODManager.PlayTextClickSound();
                data.DialogueLines[i].speakEvent?.Invoke();
            }

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

                Managers.UI.ClosePopupUI(this);
            }
        }

        private void HandleChoiceBtn(int index)
        {
            Main.Runtime.Manager.Managers.FMODManager.PlayTextClickSound();

            DialogueSO nextDialogue = _currentData.dialogueChoiseSOList[index].dialogueSO;

            for (int i = 0; i < _currentData.dialogueChoiseSOList.Count; i++)
                _choiseButtonList[i].Item1.gameObject.SetActive(false);


            StartCoroutine(ShowTextCoroutine(nextDialogue));
        }
    }
}