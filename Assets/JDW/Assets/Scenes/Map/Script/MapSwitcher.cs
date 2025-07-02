using System.Collections;
using System.Collections.Generic;

using ProjectVS;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSwitcher : MonoBehaviour
{
    
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
}
