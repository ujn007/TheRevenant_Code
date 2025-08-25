using UnityEngine;

namespace KHJ.Tool
{
    public class DestroyDisabledObjects : MonoBehaviour
    {
        public void DestoryDisableObj()
        {
            int count = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                if (obj.activeSelf)
                {
                    DestroyImmediate(obj);
                    count++;
                }
            }

            Debug.Log($"{count}개의 오브젝트를 삭제했습니다.");
        }
    }
}
