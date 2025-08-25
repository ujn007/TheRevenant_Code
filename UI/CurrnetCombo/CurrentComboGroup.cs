using DG.Tweening;
using Main.Core;
using Main.Runtime.Manager;
using PJH.Runtime.Players;
using System.Collections;
using UnityEngine;
using Debug = Main.Core.Debug;

namespace KHJ.UI
{
    public class CurrentComboGroup : MonoBehaviour
    {
        [SerializeField] private float _moveAnchoredSpeed;
        [SerializeField] private float _comboUIInterval;
        private CurrentComboUI _currentComboUI;
        private PlayerCommandActionManager _commandManager;
        private Player _player;

        private IEnumerator Start()
        {
            _player = PlayerManager.Instance.Player as Player;
            _commandManager = _player.GetCompo<PlayerCommandActionManager>();
            _commandManager.OnUseCommandActionIndex += HandleChangeComboEvent;

            yield return null;
            SetComboUI(0);
        }

        private void OnDestroy()
        {
            _commandManager.OnUseCommandActionIndex -= HandleChangeComboEvent;
        }

        private void HandleChangeComboEvent(int comboIndex)
        {
            if (_currentComboUI != null)
                SetComboUI(comboIndex);
        }

        private void SetComboUI(int comboIndex)
        {
            if (_currentComboUI != null)
            {
                Sequence outSq = DOTween.Sequence();
                _currentComboUI.DestroyCombo();
                outSq.Append(_currentComboUI.rect.DOAnchorPosY(-_comboUIInterval, _moveAnchoredSpeed));
                outSq.Join(_currentComboUI.CanvasGroup.DOFade(0, _moveAnchoredSpeed));
                outSq.OnComplete(() =>
                {
                    if (_currentComboUI != null)
                        Destroy(_currentComboUI.gameObject);
                });
                outSq.SetLink(gameObject);
            }

            var s = AddressableManager.Load<CurrentComboUI>("ComboIconGroup").gameObject.transform.localScale;
            CurrentComboUI comboUI = AddressableManager.Instantiate<CurrentComboUI>("ComboIconGroup", transform);
            comboUI.SetComboIcon(comboIndex);
            comboUI.rect.anchoredPosition = new Vector2(0, _comboUIInterval);

            Sequence inSq = DOTween.Sequence();
            inSq.Append(comboUI.rect.DOAnchorPos(Vector2.zero, _moveAnchoredSpeed));
            inSq.Join(comboUI.CanvasGroup.DOFade(1, _moveAnchoredSpeed).ChangeStartValue(0))
                .OnComplete(() => StartCoroutine(WaitFrame(comboUI)));
            inSq.SetLink(gameObject);
        }

        private IEnumerator WaitFrame(CurrentComboUI comboUI)
        {
            yield return null;
            _currentComboUI = comboUI;

            if (transform.childCount > 1)
                Destroy(transform.GetChild(0).gameObject);
        }
    }
}