using System.Collections.Generic;
using System.Linq;

using ProjectVS.Data.Player;
using ProjectVS.ItemData.ItemData;

using UnityEngine;

namespace ProjectVS.ItemYSJ
{
    public class ItemSetEvaluator
    {
        /// <summary>
        /// 현재 장착 아이템 리스트를 기준으로 발동된 세트 효과 목록 반환
        /// </summary>
        public static List<ItemSetEffect> EvaluateSetEffects(List<ItemDataSO> equippedItems)
        {
            Dictionary<ItemSetEffect, List<ItemDataSO>> setGroups = new();

            // 세트가 있는 아이템만 그룹핑
            foreach (var item in equippedItems)
            {
                if (item == null || item.SetEffect == ItemSetEffect.None)
                    continue;

                if (!setGroups.ContainsKey(item.SetEffect))
                    setGroups[item.SetEffect] = new List<ItemDataSO>();

                setGroups[item.SetEffect].Add(item);
            }

            // 조건을 만족하는 세트만 추출
            List<ItemSetEffect> activatedSets = new();
            foreach (var kvp in setGroups)
            {
                var setEffect = kvp.Key;
                var items = kvp.Value;

                // 어떤 아이템이든 조건 만족하는 값만 있으면 발동
                int required = items[0].SetEffectRequiredCount;
                if (items.Count >= required)
                {
                    activatedSets.Add(setEffect);
                }
            }

            return activatedSets;
        }
    }
}
