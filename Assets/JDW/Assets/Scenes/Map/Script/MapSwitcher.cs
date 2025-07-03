using System.Collections;
using System.Collections.Generic;

using ProjectVS;
using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSwitcher : MonoBehaviour
{
    public FadeManager Fade;
    void Awake()
    {
        PlayerDataManager.Instance.BattleSceneCount++;
    }
    public void OnBattleField()
   {
        //  StartCoroutine(HandleFadeTransition());
        PlayerDataManager.Instance.LoadPlayerData(0);
        Debug.Log("전투맵으로 씬 전환");
       SceneManager.LoadScene("BattleScene");
   }
  
   public void OnStoreField()
   {
        // StartCoroutine(HandleFadeTransition());
        PlayerDataManager.Instance.SavePlayerData(0);
        Debug.Log("상점맵으로 씬 전환");
       SceneManager.LoadScene("StoreScene");
   }
    private IEnumerator HandleFadeTransition()
    {
    
        yield return StartCoroutine(Fade.FadeIn());

    }
    void OnBattleSceneLoaded() // 배틀씬이 로드 될 때 배틀씬 카운트 추가 ps. 게임매니저에 포함 하거나 컴포넌트로 가지고 있어야 됌
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            PlayerDataManager.Instance.BattleSceneCount++;
        }
    }

}
