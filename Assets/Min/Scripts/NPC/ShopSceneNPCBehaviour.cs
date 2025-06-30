using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CostumeStateManagerClass = ProjectVS.CharacterImages.CostumeStateManager.CostumeStateManager;
using CostumeSOClass = ProjectVS.CharacterImages.CostumeSO.CostumeSO;
using ProjectVS.UIs.UIBase.UIButtonAnimator;


namespace ProjectVS.NPC.ShopSceneNPCBehaviour
{
    public class ShopSceneNPCBehaviour : MonoBehaviour
    {
        [SerializeField] private CostumeStateManagerClass _costumeStateManager;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_costumeStateManager == null)
            {
                _costumeStateManager = FindObjectOfType<CostumeStateManagerClass>();
            }
        }

        private void OnEnable()
        {
            RenewCostumeAnimation();
        }

        // 옷을 입거나 벗었을 때에도 호출
        public void RenewCostumeAnimation()
        {
            var currentCostume = _costumeStateManager.CurrentCostume;
            if (currentCostume == null)
            {
                _animator.SetTrigger("Default");
                Debug.Log($"[ShopSceneNPCBehaviour] Default 호출됨");
            }
            else
            {
                if (HasTrigger(currentCostume.CostumeName))
                {
                    _animator.SetTrigger(currentCostume.CostumeName);
                    Debug.Log($"[ShopSceneNPCBehaviour] {currentCostume.CostumeName} SetTrigger됨");
                }
                else
                {
                    Debug.LogWarning($"[ShopSceneNPCBehaviour] Trigger {currentCostume.CostumeName}가 존재하지 않음");
                }
            }
        }

        private bool HasTrigger(string triggerName)
        {
            foreach (AnimatorControllerParameter param in _animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
