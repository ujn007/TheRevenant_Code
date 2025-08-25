using UnityEngine;

namespace KHJ.Tool
{
    public class ColliderRemover : MonoBehaviour
    {
        public void RemoveCollidersInChildren()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            int count = 0;

            foreach (var col in colliders)
            {
                if (col is MeshCollider || col is BoxCollider)
                {
                    DestroyImmediate(col);
                    count++;
                }
            }

            Debug.Log($"{count}���� MeshCollider �Ǵ� BoxCollider�� �����߽��ϴ�.");
        }
    }
}
