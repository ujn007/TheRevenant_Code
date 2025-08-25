using KHJ.Tool;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DestroyDisabledObjects))]
public class DestroyDisabledObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DestroyDisabledObjects script = (DestroyDisabledObjects)target;

        if (GUILayout.Button("비활성화 된 오브젝트 삭제"))
        {
            script.DestoryDisableObj();
        }
    }
}

