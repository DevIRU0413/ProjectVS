using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVS.UIs.PanelBehaviours.CharacterIndicator
{
    public class CharacterIndicator : MonoBehaviour
    {
        [SerializeField] private List<Animator> _characterModels;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ChangeIndex(int value)
        {
            if (value < 0 || value >= _characterModels.Count)
            {
                Debug.LogError($"[CharacterIndicator] 인덱스 {value}는 유효하지 않음");
                return;
            }

            for (int i = 0; i < _characterModels.Count; i++)
            {
                _characterModels[i].gameObject.SetActive(i == value);
            }
        }

        // 선택 시 호출할 메서드
        public void AnimateAttack(int value)
        {
            // 추후에 호출 방식 바꿔야 될 수도
            _characterModels[value].Play("Attack");
        }
    }
}
