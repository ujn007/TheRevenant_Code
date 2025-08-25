using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KHJ.UI
{
    public class ComboPassiveUI : MonoBehaviour
    {
        private List<PassiveFrame> _frameList = new();

        private void Awake()
        {
            _frameList = GetComponentsInChildren<PassiveFrame>().ToList();
        }
    }
}
