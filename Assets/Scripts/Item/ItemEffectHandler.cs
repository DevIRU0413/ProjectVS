using ProjectVS.Data;

using UnityEngine;

namespace ProjectVS.Item
{
    public class ItemEffectHandler
    {
        // 단순 힐 같이 단발성 아이템도 존재하기에 따로 뺴서 관리
        // 인벤에 귀속 기능 x
        public static void ApplyEffect(Unit.Player.PlayerStats stats, ItemData item)
        {
            if (item == null || stats == null) return;

            switch (item.ItemEffect)
            {
                case ItemEffect.HPStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.MaxHp, item.ItemEffectValue);
                    break;

                case ItemEffect.DefStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.Dfs, item.ItemEffectValue);
                    break;

                case ItemEffect.SpeedStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.Spd, item.ItemEffectValue);
                    break;

                case ItemEffect.Restore:
                    stats.CurrentHp += item.ItemEffectValue;
                    break;

                // 공격형은 스킬 시스템과 연동해서 따로 처리
                case ItemEffect.Swing:
                case ItemEffect.Shot:
                case ItemEffect.ThreeShot:
                case ItemEffect.Tornado:
                case ItemEffect.Eightway:
                    Debug.Log($"[ItemEffect] 공격형 효과는 별도 시스템에서 적용 필요: {item.ItemEffect}");
                    break;

                default:
                    Debug.Log($"[ItemEffect] 처리되지 않은 효과: {item.ItemEffect}");
                    break;
            }
        }
    }
}
