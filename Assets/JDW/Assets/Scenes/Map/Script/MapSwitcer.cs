using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcer : MonoBehaviour
{
    public GameObject battleField;
    public GameObject storeField;

     public GameObject Tilemap1;
     public GameObject Tilemap2;
     public GameObject Tilemap3;
     public GameObject Tilemap4;


    public void OnBattleField()
    {
        Debug.Log("맵전환 실행");

        if (battleField == null || storeField == null)
        {
            Debug.LogError("❌ battleField 또는 storeField가 null입니다.");
            return;
        }

        Debug.Log($"[전환 전] battleField.activeSelf = {battleField.activeSelf}, storeField.activeSelf = {storeField.activeSelf}");

        battleField.SetActive(true);
        storeField.SetActive(false);

        Debug.Log($"[전환 후] battleField.activeSelf = {battleField.activeSelf}, storeField.activeSelf = {storeField.activeSelf}");
    }
    public bool IsBattleActive()
    {
        return battleField.activeSelf;
    }
    public void OnstoreField()
    {
        Debug.Log("맵전환 실행");
        battleField.SetActive(false);
        storeField.SetActive(true);
    }
    public bool IsStoreActive()
    {
        return storeField.activeSelf;
    }
    public void ResetTileMap()
    {
        Tilemap1.transform.position = new Vector3(24f, 11f, 0f);
        Tilemap2.transform.position = new Vector3(-14f, 11f, 0f);
        Tilemap3.transform.position = new Vector3(-16f, -29f, 0f);
        Tilemap4.transform.position = new Vector3(26f, -29f, 0f);
    }
}
