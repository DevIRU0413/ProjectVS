using ProjectVS.Data.Player;
using ProjectVS.Item;
using ProjectVS.ItemData.ItemData;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerItemEquipSystem : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private ProjectileLauncher _projectileLauncher;

        private PlayerStats stats;

        private void Awake()
        {
            stats = _config.Stats;
        }

        /// <summary>
        /// 아이템 획득 시 호출됨
        /// </summary>
        public void EquipItem(ItemDataSO item)
        {
            if (item == null) return;

            switch (item.ItemType)
            {
                case ItemType.Passive:
                case ItemType.Potion:
                    ApplyPassiveEffect(item);
                    break;

                case ItemType.Attack:
                    EquipWeapon(item);
                    break;

                default:
                    Debug.Log($"[EquipSystem] 처리 대상 아님: {item.ItemName}");
                    break;
            }
        }

        private void ApplyPassiveEffect(ItemDataSO item)
        {
            ItemEffectHandler.ApplyEffect(stats, item);
            Debug.Log($"[EquipSystem] 패시브 효과 적용: {item.ItemName}");
        }

        private void EquipWeapon(ItemDataSO item)
        {
            _projectileLauncher?.RegisterWeapon(item);
            Debug.Log($"[EquipSystem] 무기 장착: {item.ItemName}");
        }
    }
}

