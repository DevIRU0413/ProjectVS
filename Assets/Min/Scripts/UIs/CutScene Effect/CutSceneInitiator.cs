using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.UIs.CutSceneEffect.CutSceneInitiator
{
    public class CutSceneInitiator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _cutSceneObjects = new();
        private List<int> _enabledList = new();

        private void Awake()
        {
            if (_cutSceneObjects.Count == 0)
            {
                Debug.LogWarning("[CutSceneInitiator] 이미지 리스트가 비어있습니다");
            }

            foreach (var image in _cutSceneObjects)
            {
                _enabledList.Add(image.activeSelf ? 1 : 0);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _cutSceneObjects.Count; i++)
            {
                _cutSceneObjects[i].SetActive(_enabledList[i] == 1);
            }
        }
    }
}
