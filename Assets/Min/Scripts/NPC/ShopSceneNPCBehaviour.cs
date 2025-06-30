using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;
using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;


namespace ProjectVS.NPC.ShopSceneNPCBehaviour
{
    public class ShopSceneNPCBehaviour : MonoBehaviour
    {
        [SerializeField] private CostumeStateManagerClass _costumeStateManager;

        private Dictionary<string, Animator> _costumeAnimations;


        private void Awake()
        {
            if (_costumeStateManager == null)
            {
                _costumeStateManager = FindObjectOfType<CostumeStateManagerClass>();
            }

            _costumeAnimations = new();

            foreach (var anim in _costumeAnimations)
            {
                if (!_costumeAnimations.ContainsKey(anim.Key))
                {
                    _costumeAnimations.Add(anim.Key, anim.Value);
                }
            }
        }

        private void OnEnable()
        {
            RenewCostumeAnimation();
        }

        // 옷을 입거나 벗었을 때에도 호출
        private void RenewCostumeAnimation()
        {
            foreach (var costume in _costumeStateManager.CostumeSOs)
            {

            }
        }
    }
}
