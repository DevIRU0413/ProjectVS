using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.SceneSingleton;

using UnityEngine;


namespace ProjectVS.UIs.CutSceneEffect.CutSceneController
{
    public enum CutSceneType
    {
        Opening,
        Intro,
        NormalEnding,
        TrueEnding
    }

    public class CutSceneController : SceneSingleton<CutSceneController>
    {
        [SerializeField] private List<CutSceneTypeObjectPair> _cutSceneObjects;

        [SerializeField] private GameObject _battlePanel;
        [SerializeField] private GameObject _itemInventoryPanel;

        [System.Serializable]
        public class CutSceneTypeObjectPair
        {
            public CutSceneType Type;
            public GameObject CutSceneObject;
        }

        public void PlayCutScene(CutSceneType type)
        {
            _battlePanel.SetActive(false);
            _itemInventoryPanel.SetActive(false);


            var target = _cutSceneObjects.Find(p => p.Type == type);
            if (target != null && target.CutSceneObject != null)
            {
                target.CutSceneObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[CutSceneController] CutSceneType {type}이 존재하지 않음");
            }
        }
    }
}
