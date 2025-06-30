using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace KHJ.UI
{
    public class MiniMapUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
    {
        [Header("MiniMapInfo")]
        [SerializeField] private Camera targetCam;
        [SerializeField] private float minCamSize, maxCamSize;
        [SerializeField] private float scrollStrValue;
        [SerializeField] private float minDragSpeed, maxDragSpeed;

        private Vector3 _maxCamVertexPos, _minCamVertexPos;
        private Vector2 _lastMousePosition;
        private bool _isEnterMap = false;

        private void Start()
        {
            targetCam.orthographicSize = maxCamSize;

            Vector3 targetPos = targetCam.transform.position;
            _maxCamVertexPos = new Vector3(targetPos.x + targetCam.orthographicSize, 0, targetPos.y + targetCam.orthographicSize);
            _maxCamVertexPos = new Vector3(targetPos.x - targetCam.orthographicSize, 0, targetPos.y - targetCam.orthographicSize);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastMousePosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 delta = eventData.position - _lastMousePosition;
            _lastMousePosition = eventData.position;

            float t = Mathf.InverseLerp(minCamSize, maxCamSize, targetCam.orthographicSize);
            float dragSpeed = Mathf.Lerp(minDragSpeed, maxDragSpeed, t);
            Vector3 move = new Vector3(-delta.x, 0f, -delta.y) * dragSpeed;

            Vector3 newPosition = targetCam.transform.position + move;
            newPosition.y = 50f;

            targetCam.transform.position = newPosition;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _isEnterMap = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isEnterMap = false;
        }

        private void Update()
        {
            Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
            float scrollY = scrollDelta.y;

            targetCam.orthographicSize -= scrollY * scrollStrValue;
            targetCam.orthographicSize = Mathf.Clamp(targetCam.orthographicSize, minCamSize, maxCamSize);
        }

        public void EndDrag()
        {

        }
    }
}
