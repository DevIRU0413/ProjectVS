using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.CharacterImages.StandingToFullBodyShotPair
{
    [System.Serializable]
    public class StandingToFullBodyShotPair
    {
        public Button StandingShotButton;
        public GameObject FullBodyShotPrefab; // 해당 프리팹은 [UI Panel, UI Button On Panel, UI Button Group Controller, UI Animator] 등록하고 UI Manager 관리 하에 사용할 듯
    }
}
