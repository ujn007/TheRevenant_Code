using UnityEngine;
using UnityEditor;
using KHJ.Tool;

[CustomEditor(typeof(ColliderRemover))]
public class ColliderRemoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColliderRemover script = (ColliderRemover)target;

        if (GUILayout.Button("MeshCollider & BoxCollider ªË¡¶"))
        {
            script.RemoveCollidersInChildren();
        }
    }
}
