using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;
using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using UnityEngine.UI;
using CostumeBuyButtonBehaviourClass = ProjectVS.UIs.CostumeBuyButtonBehaviour.CostumeBuyButtonBehaviour;

namespace ProjectVS.UIs.PanelBehaviours.CostumeChangePanelBehaviour
{
    public class CostumeChangePanelButtons : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentBox;
        [SerializeField] private GameObject _costumeButtonPrefab;

        [SerializeField] private CostumeStateManagerClass _costumeStateManager;

        private void Start()
        {
            foreach (var costumeSO in _costumeStateManager.CostumeSOs)
            {
                GameObject buyButtton = Instantiate(_costumeButtonPrefab, _contentBox);
                Image image = buyButtton.GetComponentInChildren<Image>();
                image.sprite = costumeSO.IconSprite;

                var buttonBehaviour = buyButtton.GetComponent<CostumeBuyButtonBehaviourClass>();
                buttonBehaviour.Init(costumeSO, _costumeStateManager);
            }
        }
    }
}
