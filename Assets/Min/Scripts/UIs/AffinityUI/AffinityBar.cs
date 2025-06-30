using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using NPCAffinityModelClass = ProjectVS.Shop.NPCAffinityModel.NPCAffinityModel;


namespace ProjectVS.UIs.AffinityBar
{
    public class AffinityBar : MonoBehaviour
    {
        [Header("호감도 바 참조")]
        [SerializeField] Image _affinityFillImage;
        [SerializeField] TMP_Text _affinityValueText;

        [Header("호감도 모델 참조")]
        [SerializeField] NPCAffinityModelClass _affinityModel;

        private void OnEnable()
        {
            RenewAffinity();
        }


        private void RenewAffinity()
        {
            _affinityValueText.text = _affinityModel.AffinityLevelString; // 레벨만 표시

            float fillAmount = (float)_affinityModel.AffinityCurrentExp / _affinityModel.AffinityExpMax;
            _affinityFillImage.fillAmount = fillAmount;
        }
    }
}
