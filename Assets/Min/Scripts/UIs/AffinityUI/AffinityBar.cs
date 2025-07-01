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
        [SerializeField] Image _affinityFillImage;
        [SerializeField] TMP_Text _affinityValueText;


        private void OnEnable()
        {
            RenewAffinity();
        }


        private void RenewAffinity()
        {
            _affinityValueText.text = NPCAffinityModel.Instance.AffinityLevelString; // 레벨만 표시

            float fillAmount = (float)NPCAffinityModel.Instance.AffinityCurrentExp / NPCAffinityModel.Instance.AffinityExpMax;
            _affinityFillImage.fillAmount = fillAmount;
        }
    }
}
