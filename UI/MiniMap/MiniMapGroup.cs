using DG.Tweening;
using LJS.Map;
using NUnit.Framework;
using PJH.Runtime.Players;
using System;
using System.Collections.Generic;
using Main.Core;
using UnityEngine;

namespace KHJ.UI
{
    public class MiniMapGroup : MonoBehaviour
    {
        private PlayerInputSO playerInputSO;
        [SerializeField] private MiniMapUI miniMapUI;
        [SerializeField] private CanvasGroup miniMapCanva;
        [SerializeField] private float fadeDuration;

        private bool _isOpenClose = false;

        private void Awake()
        {
            playerInputSO = AddressableManager.Load<PlayerInputSO>("PlayerInputSO");
            playerInputSO.MapOpenCloseEvent += HandleMapOpenClose;
        }

        private void OnDestroy()
        {
            playerInputSO.MapOpenCloseEvent -= HandleMapOpenClose;
        }

        private void Start()
        {
        }

        private void HandleChangeCurrentRoomEvent()
        {
        }

        private void HandleMapOpenClose()
        {
            _isOpenClose = !_isOpenClose;

            if (!_isOpenClose)
                miniMapUI.EndDrag();

            playerInputSO.EnablePlayerInput(!_isOpenClose);
            int alphaValue = _isOpenClose ? 1 : 0;
            DOTween.To(() => miniMapCanva.alpha, x => miniMapCanva.alpha = x, alphaValue, fadeDuration);
        }
    }
}