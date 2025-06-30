using Main.Runtime.Manager;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.UI
{
    public class PlayerArrowIcon : MonoBehaviour
    {
        private Transform playerModel;
        private RectTransform rectTransform;

        private void Start()
        {
            Player player = PlayerManager.Instance.Player as Player;
            rectTransform = transform as RectTransform;
            playerModel = player.ModelTrm;
        }

        private void LateUpdate()
        {
            float yRotation = playerModel.eulerAngles.y;
            rectTransform.localEulerAngles = new Vector3(0, 0, -yRotation);
        }
    }
}