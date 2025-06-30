using System;
using UnityEngine;
using UnityEngine.Events;

namespace KHJ.Dialogue
{
    [Serializable]
    public struct DialogueData
    {
        public string speaker;
        [TextArea(4, 3)] public string contents;
        public UnityEvent speakEvent;
    }
}
