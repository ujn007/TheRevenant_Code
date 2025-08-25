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

        if (GUILayout.Button("��Ȱ��ȭ �� ������Ʈ ����"))
        {
            script.DestoryDisableObj();
        }
    }
}

