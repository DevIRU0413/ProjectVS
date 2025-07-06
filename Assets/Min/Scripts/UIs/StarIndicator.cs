using System.Collections;
using System.Collections.Generic;

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

        private void RenewStar()
        {
            // 한 번 확인해봐야 될 듯
            //_starText.text = $"{PlayerDataManager.Instance.Star}";
        }
    }
}
