using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.UIManager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;
using ProjectVS.UIs.PanelBehaviours.BuyCheckPanelButtons;
using ProjectVS.Interface;


namespace ProjectVS.UIs.CostumeBuyButton
{
    public class CostumeBuyButton : MonoBehaviour
    {
        [SerializeField] private GameObject _lockedImage;
        [SerializeField] private Image _costumeIconImage;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _costumeNameText;
        [SerializeField] private GameObject _starImage;


        private CostumeSOClass _costume;
        private CostumeStateManagerClass _manager;

        private void Start()
        {
            SetPrice();
            CheckUnlocked();
        }

        public void Init(CostumeSOClass costume, CostumeStateManagerClass manager)
        {
            _costume = costume;
            _manager = manager;
        }

        public void OnClickBuyButton()
        {
            _manager.WillBuyCostume(_costume, this);
            UIManager.Instance.Show("Buy Check Panel");

            CheckUnlocked();
        }

        public void OnToggleWearButton()
        {
            _manager.ToggleOrPurchaseCostume(_costume);
        }

        public void CheckUnlocked()
        {
            if (_costume.IsUnlocked)
            {
                _lockedImage.SetActive(false);
                _priceText.gameObject.SetActive(false);
                _costumeIconImage.sprite = _costume.IconSprite;
                _starImage.SetActive(false);
                _costumeNameText.gameObject.SetActive(true);
                _costumeNameText.text = _costume.CostumeName;
            }
            else
            {
                _lockedImage.SetActive(true);
                _priceText.gameObject.SetActive(true);
                _starImage.SetActive(true);
                _costumeNameText.gameObject.SetActive(false);
            }
        }

        private void SetPrice()
        {
            _priceText.text = _costume.Price.ToString();
        }
    }
}
