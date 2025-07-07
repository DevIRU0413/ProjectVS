using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using TMPro;

using UnityEngine;


namespace ProjectVS.UIs.StarIndicator
{
    public class StarIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _starText;

        private void OnEnable()
        {
            RenewStar();
        }

        public void RenewStar()
        {
            _starText.text = $"{PlayerDataManager.Instance.Diamonds}";
        }
    }
}
