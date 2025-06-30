using System;
using System.Collections;

using ProjectVS.Util;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectVS.Manager
{
    public class SceneLoader : SimpleSingleton<SceneLoader>
    {
        public event Action<string> OnSceneLoaded;

        private float _loadProgress;
        public float LoadProgress => _loadProgress;

        public void LoadSceneAsync(SceneID id)
        {
            string sceneName = id.ToString();
            StartCoroutine(LoadRoutine(sceneName));
        }

        private IEnumerator LoadRoutine(string sceneName)
        {
            _loadProgress = 0f;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                _loadProgress = operation.progress / 0.9f;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            _loadProgress = 1f;
            operation.allowSceneActivation = true;

            yield return new WaitUntil(() => operation.isDone);

            OnSceneLoaded?.Invoke(sceneName);
        }
    }
}
