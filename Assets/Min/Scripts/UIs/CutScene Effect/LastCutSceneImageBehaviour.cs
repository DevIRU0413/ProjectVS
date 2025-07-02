using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.UIs.CutSceneEffect.LastCutSceneImageBehaviour
{
    public class LastCutSceneImageBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _openingScene;
        [SerializeField] private GameObject _introScene;
        [SerializeField] private GameObject _normalEndScene;
        [SerializeField] private GameObject _trueEndScene;

        public void OnClickLastOpeningImage()
        {
            // 캐릭터 선택 끝나고
            _openingScene.SetActive(false);
        }

        public void OnClickLastIntroImage()
        {
            // 인게임 씬 들어가자마자
            _introScene.SetActive(false);
        }

        public void OnClickLastNormalEndImage()
        {
            // 노말엔딩 끝나고 호출해야될 것 여기서 호출
            _normalEndScene.SetActive(false);
        }

        public void OnClickLastTrueEndImage()
        {
            // 트루엔딩 끝나고 호출해야될 것 여기서 호출
            _trueEndScene.SetActive(false);
        }
    }
}
