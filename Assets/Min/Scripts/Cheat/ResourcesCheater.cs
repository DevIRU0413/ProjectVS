using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using TMPro;

using UnityEngine;


namespace ProjectVS.Cheat.ResourcesCheater
{
    public class ResourcesCheater : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourcesText;

        private void OnEnable()
        {
            RenewText();
        }

        public void OnClickGoldUp()
        {
            PlayerDataManager.Instance.Gold += 100;

            RenewText();
        }

        public void OnClickGoldDown()
        {
            if (PlayerDataManager.Instance.Gold - 100 < 0)
                PlayerDataManager.Instance.Gold = 0;
            else
                PlayerDataManager.Instance.Gold -= 100;

            RenewText();
        }

        public void OnClickDiamondUp()
        {
            PlayerDataManager.Instance.Diamonds += 100;

            RenewText();
        }

        public void OnClickDiamondDown()
        {
            if (PlayerDataManager.Instance.Diamonds - 100 < 0)
                PlayerDataManager.Instance.Diamonds = 0;
            else
                PlayerDataManager.Instance.Diamonds -= 100;

            RenewText();
        }

        private void RenewText()
        {
            _resourcesText.text = $"골드: {PlayerDataManager.Instance.Gold}, 다이아 {PlayerDataManager.Instance.Diamonds}";
        }
    }
}
