using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KHJ
{
    public class DontDestroy : MonoBehaviour
    {
        private bool isBaseUI;

        private void Awake()
        {
            SceneManager.sceneLoaded += HandleSceneLoad;
        }

        private void HandleSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            DontDestroy[] instances = FindObjectsByType<DontDestroy>(FindObjectsSortMode.None);

            if (instances.Length > 1)
            {
                if (!isBaseUI)
                    Destroy(gameObject);
            }
            else
                DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            isBaseUI = true;
        }
    }
}