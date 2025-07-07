using System.Collections;
using System.Collections.Generic;

using ProjectVS.Shop.NPCAffinityModel;

using TMPro;

using UnityEngine;


namespace ProjectVS.Cheat.AffinityCheater
{
    public class AffinityCheater : MonoBehaviour
    {
        [SerializeField] private TMP_Text _affinityText;

        private void OnEnable()
        {
            RenewAffinityText();
        }

        public void OnClickDownEXP()
        {
            NPCAffinityModel.Instance.TestDownAffinity();
            RenewAffinityText();
        }

        public void OnClickUpEXP()
        {
            NPCAffinityModel.Instance.TestUpAffinity();
            RenewAffinityText();
        }

        public void OnClickFullUpLvl()
        {
            NPCAffinityModel.Instance.TestFullUpAffinity();
            RenewAffinityText();
        }

        private void RenewAffinityText()
        {
            _affinityText.text = $"호감도 레벨: {NPCAffinityModel.Instance.AffinityLevel} \n 호감도 경험치: {NPCAffinityModel.Instance.AffinityCurrentExp}";
        }
    }
}
