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

            Debug.Log($"{count}개의 MeshCollider 또는 BoxCollider를 삭제했습니다.");
        }
    }
}
