using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Shop.NPCAffinityModel;


namespace ProjectVS.UIs.AffinityBar
{
    public class AffinityBar : MonoBehaviour
    {
        [Header("호감도 바 참조")]
        [SerializeField] private TMP_Text _affinityValueText;
        [SerializeField] private Image _image;


        private void OnEnable()
        {
            RenewAffinity();
        }

        
        private void RenewAffinity()
        {
            Vector3 v3 = _image.rectTransform.localScale;
            v3.x = (float)NPCAffinityModel.Instance.AffinityCurrentExp / NPCAffinityModel.Instance.AffinityExpMax;
            _image.rectTransform.localScale = v3;

            _affinityValueText.text = NPCAffinityModel.Instance.AffinityLevelString; // 레벨만 표시
        }
    }
}
