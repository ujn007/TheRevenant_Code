using KHJ.Tool;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialShaderChanger))]
public class MaterialShaderChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MaterialShaderChanger script = (MaterialShaderChanger)target;

        if (GUILayout.Button("Lit → Unlit 변경 (복사 또는 재사용)"))
        {
            script.ChangeLitToUnlitInChildren();
        }
    }
}

