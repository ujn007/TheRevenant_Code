#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace KHJ.Data
{
    public class SOFinder
    {
        public static T GetScriptableObject<T>(string folderPath, string soName) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T so = AssetDatabase.LoadAssetAtPath<T>(path);

                if (so != null && so.name == soName)
                {
                    return so;
                }
            }
            return null;
        }
    }
}
#endif
