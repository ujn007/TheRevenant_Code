using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace KHJ.Tool
{
    public class MaterialShaderChanger : MonoBehaviour
    {
        private const string targetShader = "Universal Render Pipeline/Lit";
        private const string newShader = "Universal Render Pipeline/Unlit";
        private const string savePath = "Assets/00Work/KHJ/05.Material/Minimap";

        public void ChangeLitToUnlitInChildren()
        {
#if UNITY_EDITOR

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
                AssetDatabase.Refresh();
            }

            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            int changedCount = 0;
            int reusedCount = 0;

            foreach (var mr in meshRenderers)
            {
                Material[] materials = mr.sharedMaterials;
                bool modified = false;

                for (int i = 0; i < materials.Length; i++)
                {
                    Material mat = materials[i];
                    if (mat != null && mat.shader != null && mat.shader.name == targetShader)
                    {
                        string fileName = $"{mat.name}_Unlit.mat";
                        string assetPath = Path.Combine(savePath, fileName).Replace("\\", "/");

                        Material existing = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                        if (existing != null)
                        {
                            materials[i] = existing;
                            reusedCount++;
                        }
                        else
                        {
                            Material newMat = new Material(mat);
                            newMat.shader = Shader.Find(newShader);

                            AssetDatabase.CreateAsset(newMat, assetPath);
                            materials[i] = newMat;
                            changedCount++;
                        }
                        modified = true;
                    }
                }

                if (modified)
                    mr.sharedMaterials = materials;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"새로 생성: {changedCount}개, 재사용: {reusedCount}개");
#endif
        }
    }
}
