using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectVS.UIs.IntroSceneToMainMenu
{
    public class IntroSceneToMainMenu : MonoBehaviour
    {
        public void OnClickLastImage()
        {
            SceneManager.LoadScene(1); // 메인 메뉴 씬이 1번 씬이라고 가정
        }
    }
}
