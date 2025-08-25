using KHJ.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KHJ
{
    public class SynergyUIManager : MonoBehaviour
    {
        [SerializeField] private InventoryInfoSO inventoryInfoSO;

        [field: SerializeField] public RectTransform synergyContainer { get; private set; }
        [field: SerializeField] public RectTransform serveSynergyContainer { get; private set; }
        [field: SerializeField] public SynergyBlockHandler synergyBlockHandler { get; private set; }
        [field: SerializeField] public RectTransform blockContainer { get; private set; }

        [SerializeField] private RectTransform firstSlot, endSlot;

        private EventSystem eventSystem;

        public static SynergyUIManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            GetFisrtAndEnd();
        }

        public void GetFisrtAndEnd()
        {
            firstSlot = inventoryInfoSO.fistSlot;
            endSlot = inventoryInfoSO.endSlot;
        }

        public Vector2Int GetGridPos(Vector2 pos = default)
        {
            if (pos == default)
                pos = GetMousePos();

            if (!IsWithinGridBounds(pos)) return Vector2Int.down;

            int childCount = inventoryInfoSO.childCount - 1;
            int childchildCount = inventoryInfoSO.childchildCount - 1;

            float normalizedMouseX = Mathf.Lerp(0, childCount,
                Mathf.InverseLerp(firstSlot.position.x, endSlot.position.x, pos.x));
            float normalizedMouseY = Mathf.Lerp(0, childchildCount,
                Mathf.InverseLerp(firstSlot.position.y - firstSlot.sizeDelta.y,
                    endSlot.position.y - endSlot.sizeDelta.y, pos.y));

            return new Vector2Int((int)normalizedMouseX, (int)normalizedMouseY);
        }

        public bool IsWithinGridBounds(Vector2 pos)
        {
            float halfLenght = firstSlot.sizeDelta.x / 2;
            return !(pos.x < firstSlot.position.x - halfLenght ||
                     pos.x > endSlot.position.x + endSlot.sizeDelta.x + halfLenght ||
                     pos.y > endSlot.position.y + halfLenght ||
                     pos.y < firstSlot.position.y - firstSlot.sizeDelta.y - halfLenght);
        }

        public Vector2 GetMousePos()
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            return pointerData.position;
        }
    }
}