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
        battleField.SetActive(true);
        storeField.SetActive(false);
    }
    public bool IsBattleActive()
    {
        return battleField.activeSelf;
    }
    public void OnstoreField()
    {
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
