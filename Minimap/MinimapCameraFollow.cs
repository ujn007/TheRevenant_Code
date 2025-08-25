using Main.Runtime.Manager;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.UI
{
    public class MinimapCameraFollow : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private Vector3 clampPoint1, clampPoint2;  
        private Player _player;

        private void Start()
        {
            _player = PlayerManager.Instance.Player as Player;
        }

        private void LateUpdate()
        {
            Vector3 newPos = _player.transform.position;
            newPos.y = _height;
            transform.position = newPos;

            float minX = Mathf.Min(clampPoint1.x, clampPoint2.x);
            float maxX = Mathf.Max(clampPoint1.x, clampPoint2.x);
            float minZ = Mathf.Min(clampPoint1.z, clampPoint2.z);
            float maxZ = Mathf.Max(clampPoint1.z, clampPoint2.z);

            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
    }
}
