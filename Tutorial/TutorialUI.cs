using BIS.Data;
using BIS.Manager;
using DG.Tweening;
using KHJ.Dialogue;
using Main.Runtime.Core.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace KHJ.Tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        private GameEventChannelSO eventSO;
        [SerializeField] private DialogueSO dialogueSO;

        [Header("UI")] [ColorUsage(true)] public Color color;
        [SerializeField] private TextMeshProUGUI nameDesText;

        [SerializeField] private RectTransform amountImageContainer;
        [SerializeField] private AmountUI amountImagePF;

        private CanvasGroup canvasGroup;
        private List<AmountUI> imageList = new();

        private void Awake()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
            eventSO = Managers.Resource.Load<GameEventChannelSO>("GameEventChannel");
            eventSO.AddListener<TutorialInfo>(HandleUpdateUI);
        }

        private void HandleUpdateUI(TutorialInfo info)
        {
            if (info.IsStartQuest)
            {
                ResetUI();
                for (int i = 0; i < info.RequiredAmount; i++)
                    imageList.Add(Instantiate(amountImagePF, amountImageContainer));

                canvasGroup.DOFade(1, 0.5f);
                EnableColor(false);
            }
            else
                imageList[info.CurrentAmount - 1].EnableImage(true);

            nameDesText.text = $"{info.QuestName} <size=26><color=#C3C3C3><i>{info.Description}</i></color></size>";

            print($"IsEnd : {info.IsEndQuest}");
            if (info.CurrentAmount >= info.RequiredAmount)
                TweenUI(info.IsEndQuest);
        }

        private void TweenUI(bool isEnd)
        {
            EnableColor(true);

            Sequence sq = DOTween.Sequence();
            sq.AppendInterval(1);
            sq.Append(nameDesText.rectTransform.DOAnchorPosX(-600, 0.2f).SetEase(Ease.InBack));
            sq.Append(amountImageContainer.DOAnchorPosX(-600, 0.2f).SetEase(Ease.InBack));
            sq.AppendCallback(() => CheckEndQuest(sq, isEnd));
            sq.AppendInterval(0.5f);
            sq.Append(nameDesText.rectTransform.DOAnchorPosX(23, 0.2f).SetEase(Ease.OutBack));
            sq.Append(amountImageContainer.DOAnchorPosX(23, 0.2f).SetEase(Ease.OutBack));
        }

        private void CheckEndQuest(Sequence sq, bool isEnd)
        {
            print($"맞미ㅏㄱ이다다다다다 {isEnd}");
            if (isEnd)
                canvasGroup.DOFade(0, 0.5f);

            EnableColor(false);
        }

        private void EnableColor(bool v)
        {
            Color color = v ? this.color : Color.white;
            nameDesText.DOColor(color, 0.2f);
            imageList.ForEach(x => x.EnableScreenImage(v));
        }

        private void ResetUI()
        {
            imageList.Clear();

            if (amountImageContainer.childCount > 0)
            {
                foreach (Transform trm in amountImageContainer)
                    Destroy(trm.gameObject);
            }
        }
    }
}