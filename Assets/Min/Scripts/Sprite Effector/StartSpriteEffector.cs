using System.Collections;
using System.Collections.Generic;
using ProjectVS.Manager;

using Scripts.Scene;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectVS.CutSceneEffectors.StartSpriteEffector
{
    public class StartSpriteEffector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Fade In/Out Settings")]
        [SerializeField, Range(0f, 1f)] private float _frameRate = 0.02f;
        [SerializeField, Range(0, 255)] private int _fadeStep = 5;

        private void OnEnable()
        {
            StartCoroutine(IE_FadeInAndOut());
        }

        private IEnumerator IE_FadeInAndOut()
        {
            WaitForSeconds wait = new WaitForSeconds(_frameRate);

            Color color = _spriteRenderer.color;
            int alpha = 0;
            while (alpha < 255)
            {
                alpha += _fadeStep;
                color.a = Mathf.Clamp01(alpha / 255f);
                _spriteRenderer.color = color;
                yield return wait;
            }

            while (alpha > 0)
            {
                alpha -= _fadeStep;
                color.a = Mathf.Clamp01(alpha / 255f);
                _spriteRenderer.color = color;
                yield return wait;
            }

            //SceneManager.LoadScene(1);
            Debug.Log("다음 씬 이동");

            LoadScene();
        }

        private void LoadScene()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneID.MainMenuScene);
        }
    }
}
