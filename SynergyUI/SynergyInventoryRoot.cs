using DG.Tweening;
using Main.Runtime.Core.Events;
using System;
using Main.Core;
using Main.Runtime.Manager;
using UnityEngine;

namespace KHJ
{
    public class SynergyInventoryRoot : MonoBehaviour
    {
        RectTransform rect => transform as RectTransform;
        private GameEventChannelSO eventSO;
        [SerializeField] private float str;
        [SerializeField] private int vir;

        private void Awake()
        {
            eventSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
            eventSO.AddListener<ShakeInven>(HandleShakeUI);
        }

        private void HandleShakeUI(ShakeInven inven)
        {
            rect.DOShakePosition(0.1f, strength: str, vibrato: vir);
        }
    }
}