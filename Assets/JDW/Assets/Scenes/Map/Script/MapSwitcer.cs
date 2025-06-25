using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcer : MonoBehaviour
{
    public GameObject battleField;
    public GameObject storeField;

   
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
}
