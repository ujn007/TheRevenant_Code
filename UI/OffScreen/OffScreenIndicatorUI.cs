using DG.Tweening;
using Main.Core;
using Main.Runtime.Core.Events;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class OffScreenIndicatorUI : MonoBehaviour
    {
        private GameEventChannelSO _gameEventChannel;

        [SerializeField] private Transform _firstTarget, _secTarget;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private float _imageFadeDuration;
        [Range(0, 100)] public float _edgeOffset = 50f;
        private RectTransform _indicatorUI => transform as RectTransform;
        private Camera _cam => Camera.main;
        private Transform _target;

        private void Awake()
        {
            _gameEventChannel = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
            _gameEventChannel.AddListener<EnterNextLevel>(HandleNextLevelEvent);
        }

        private void Start()
        {
            EnableImage(false);
            _target = _firstTarget;
        }

        private void HandleNextLevelEvent(EnterNextLevel level)
        {
            EnableImage(false);
            _target = _secTarget;
        }

        private void Update()
        {
            if (_target == null || _cam == null) return;

            Vector3 viewPos = _cam.WorldToViewportPoint(_target.position);

            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 screenPos;

            bool isInside = viewPos.z > 0 &&
                            viewPos.x >= 0f && viewPos.x <= 1f &&
                            viewPos.y >= 0f && viewPos.y <= 1f;

            if (isInside)
            {
                _indicatorImage?.DOFade(0, 0.1f);
                screenPos = _cam.WorldToScreenPoint(_target.position);
            }
            else
            {
                _indicatorImage?.DOFade(1, 0.1f);
                Vector3 worldScreen = _cam.WorldToScreenPoint(_target.position);

                if (worldScreen.z < 0)
                {
                    worldScreen.x = Screen.width - worldScreen.x;
                    worldScreen.y = Screen.height - worldScreen.y;
                }

                Vector2 dir = ((Vector2)worldScreen - screenCenter).normalized;
                Rect rect = new Rect(_edgeOffset, _edgeOffset,
                                     Screen.width - _edgeOffset * 2,
                                     Screen.height - _edgeOffset * 2);

                screenPos = screenCenter;
                float tMin = float.MaxValue;

                if (dir.x < 0)
                {
                    float t = (rect.xMin - screenCenter.x) / dir.x;
                    float y = screenCenter.y + dir.y * t;
                    if (y >= rect.yMin && y <= rect.yMax && t > 0 && t < tMin)
                    {
                        tMin = t;
                        screenPos = new Vector2(rect.xMin, y);
                    }
                }
                if (dir.x > 0)
                {
                    float t = (rect.xMax - screenCenter.x) / dir.x;
                    float y = screenCenter.y + dir.y * t;
                    if (y >= rect.yMin && y <= rect.yMax && t > 0 && t < tMin)
                    {
                        tMin = t;
                        screenPos = new Vector2(rect.xMax, y);
                    }
                }
                if (dir.y < 0)
                {
                    float t = (rect.yMin - screenCenter.y) / dir.y;
                    float x = screenCenter.x + dir.x * t;
                    if (x >= rect.xMin && x <= rect.xMax && t > 0 && t < tMin)
                    {
                        tMin = t;
                        screenPos = new Vector2(x, rect.yMin);
                    }
                }
                if (dir.y > 0)
                {
                    float t = (rect.yMax - screenCenter.y) / dir.y;
                    float x = screenCenter.x + dir.x * t;
                    if (x >= rect.xMin && x <= rect.xMax && t > 0 && t < tMin)
                    {
                        tMin = t;
                        screenPos = new Vector2(x, rect.yMax);
                    }
                }
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _indicatorUI.parent as RectTransform,
                screenPos,
                null,
                out Vector2 localPos
            );
            _indicatorUI.localPosition = localPos;
            _indicatorUI.rotation = Quaternion.identity;
        }

        public void EnableImage(bool isEnable)
        {
            _indicatorImage.gameObject.SetActive(isEnable);
        }
    }
}
