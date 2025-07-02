using System.Collections;
using System.Collections.Generic;

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

    public class CutSceneController : MonoBehaviour
    {
        [SerializeField] private List<CutSceneTypeObjectPair> _cutSceneObjects;

        [System.Serializable]
        public class CutSceneTypeObjectPair
        {
            public CutSceneType Type;
            public GameObject CutSceneObject;
        }

        public void PlayCutScene(CutSceneType type)
        {
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
