using Main.Runtime.Manager;
using PJH.Runtime.Players;
using UnityEngine;

namespace KHJ.Minimap
{
    public class MinimapTwoFloor : MonoBehaviour
    {
        private Player _player;

        private void Start()
        {
            _player = PlayerManager.Instance.Player as Player;
        }

        private void Update()
        {
            
        }
    }
}
