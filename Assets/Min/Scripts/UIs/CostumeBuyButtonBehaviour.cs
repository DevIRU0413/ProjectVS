using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;


namespace ProjectVS.UIs.CostumeBuyButtonBehaviour
{
    public class CostumeBuyButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _lockedImage;
        private TMP_Text _nameText;

        private CostumeSOClass _costumeSO;
        private CostumeStateManagerClass _costumeStateManager;

        private void Start()
        {
            _nameText = _button.GetComponentInChildren<TMP_Text>();
            _nameText.text = _costumeSO.CostumeName;
            CheckUnlocked();
        }

        public void Init(CostumeSOClass costume, CostumeStateManagerClass manager)
        {
            _costumeSO = costume;
            _costumeStateManager = manager;
        }

        public void OnClickBuyButton()
        {
            _costumeStateManager.ToggleOrPurchaseCostume(_costumeSO);
            CheckUnlocked();
        }

        private void CheckUnlocked()
        {
            if (_costumeSO.IsUnlocked)
            {
                _lockedImage.enabled = false;
            }
            else
            {
                _lockedImage.enabled = true;
            }
        }
    }
}
