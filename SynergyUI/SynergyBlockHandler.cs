using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KHJ
{
    public class SynergyBlockHandler : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        public SynergyBlockParts parts;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private EventSystem eventSystem;

        private void Awake()
        {
            SceneManager.sceneLoaded += HandleLoadScene;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleLoadScene;
        }

        private void HandleLoadScene(Scene arg0, LoadSceneMode arg1)
        {
            eventSystem = FindAnyObjectByType<EventSystem>();
        }

        private void Update()
        {
            if (!CanvaEnable()) return;

            if (Input.GetMouseButtonDown(0))
            {
                SynergySlotUI hitSlot = GetUIUnderMouse();
                hitSlot?.OnPointerDown();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SynergySlotUI hitSlot = GetUIUnderMouse();
                if (hitSlot == null)
                    parts.OnPointerUp(null);
                else
                    hitSlot?.OnPointerUp();
            } 
        }

        SynergySlotUI GetUIUnderMouse()
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                
                if (result.gameObject.TryGetComponent(out SynergySlotUI slot))
                {
                    return slot;
                }
            }

            return null;
        }

        private bool CanvaEnable() => canvasGroup.alpha == 1 ? true : false;
    }
}
