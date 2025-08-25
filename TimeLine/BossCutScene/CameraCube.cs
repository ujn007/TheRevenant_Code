using UnityEngine;

namespace KHJ
{
    public class CameraCube : MonoBehaviour
    {
        private Rigidbody _rigid;

        private void Awake()
        {
             _rigid = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rigid.useGravity = false;
        }

        public void DropCamera()
        {
            _rigid.useGravity = true;
        }
    }
}
