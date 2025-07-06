using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ProjectVS.CharacterImages.CostumeSO;
using ProjectVS.Utils.UIManager;
using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;
using UnityEngine.UI;




namespace ProjectVS.UIs.PanelBehaviours.BuyCheckPanelButtons
{
    public class BuyCheckPanelButtons : MonoBehaviour
    {
        [SerializeField] private TMP_Text _costumeNameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _costumeIconImage;
        [SerializeField] private CostumeStateManagerClass _costumeStateManager;

        private CostumeSO _costume;


        private void OnEnable()
        {
            RenewPanel();
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }

        public void OnClickBuyButton()
        {
            //if (돈이 없으면) return;

            _costumeStateManager.WantToBuyCostume.IsUnlocked = true;

            if (_costumeStateManager.CostumeBuyButton == null)
            {
                Debug.Log("[BuyCheckPanelButtons] CostumeBuyButton 이 null");
            }
            else
            {
                Debug.Log("[BuyCheckPanelButtons] CostumeBuyButton 의 CheckUnlocked 호출");
                _costumeStateManager.CostumeBuyButton.CheckUnlocked();
            }

            //_costumeStateManager.CostumeBuyButton?.CheckUnlocked();
            UIManager.Instance.CloseTopPanel();
        }

        private void RenewPanel()
        {
            _costumeIconImage.sprite = _costumeStateManager.WantToBuyCostume.IconSprite;
            _costumeNameText.text = _costumeStateManager.WantToBuyCostume.CostumeName;
            _descriptionText.text = _costumeStateManager.WantToBuyCostume.CostumeDescription;
        }
    }
}
