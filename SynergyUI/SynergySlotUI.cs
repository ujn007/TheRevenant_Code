using UnityEngine;
using UnityEngine.UI;

namespace KHJ
{
    public class SynergySlotUI : MonoBehaviour
    {
        private SynergyBlockHandler handler => SynergyUIManager.Instance.synergyBlockHandler;

        [field: SerializeField] public Image markImage { get; private set; }
        [field: SerializeField] public bool isServeSlot { get; private set; }
        public Vector2Int coord { get; private set; }
        public SynergyBlockParts parts;

        public void Initialize(SynergyBlockHandler handler, Vector2Int coord)
        {
            this.coord = coord;
        }

        public void OnPointerDown()
        {
            parts?.OnPointerDown(this);
            parts = null;
        }

        public void OnPointerUp()
        {
            handler.parts?.OnPointerUp(this);
            handler.parts = null;
        }

        public void EnableMarkImage(bool isEnable)
        {
            markImage.enabled = isEnable;
        }

        public void IsServeSlot(bool isServeSlot) => this.isServeSlot = isServeSlot;
    }

}
