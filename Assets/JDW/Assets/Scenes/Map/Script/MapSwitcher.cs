using System.Collections;
using System.Collections.Generic;

using ProjectVS;
using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSwitcher : MonoBehaviour
{
    void Awake()
    {
        PlayerDataManager.Instance.battleSceneCount++;
    }
    public void OnBattleField()
   {
       Debug.Log("전투맵으로 씬 전환");
       SceneManager.LoadScene("BattleScene");
   }
  
   public void OnStoreField()
   {
       Debug.Log("상점맵으로 씬 전환");
       SceneManager.LoadScene("StoreScene");
   }
    void OnBattleSceneLoaded() // 배틀씬이 로드 될 때 배틀씬 카운트 추가 ps. 게임매니저에 포함 하거나 컴포넌트로 가지고 있어야 됌
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            PlayerDataManager.Instance.battleSceneCount++;
        }
    }
}
