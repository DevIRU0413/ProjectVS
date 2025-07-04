using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.JDW;

using UnityEngine;

using static UnityEditor.Progress;

public class TestPlayerInventory : MonoBehaviour
{
    public int MaxItemCount = 8;

    public List<TestItemData> items = new List<TestItemData>();

    public PlayerConfig PlayerConfig;

    public void AddItem(TestItemData item)
    {
        if (items.Count >= MaxItemCount)
        {
            Debug.LogWarning("인벤토리에 더 이상 아이템을 추가할 수 없습니다.");
            return;
        }
        items.Add(item);
        Debug.Log($"아이템 추가됨: {item.Name}");
        FindObjectOfType<TestInventoryDisplay>()?.UpdateInventoryUI(items);

    }
    public void DiscardItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Debug.Log("[인벤토리] 잘못된 인덱스입니다.");
            return;
        }

        var item = items[index];
        var player = GetComponent<PlayerConfig>();
        if (player == null)
        {
            Debug.LogError("[인벤토리] PlayerConfig를 찾을 수 없습니다.");
            return;
        }

        // 스탯 제거
        player.RemoveItemStats(
            item.BonusHP,
            item.BonusAtk,
            item.BonusDfs,
            item.BonusAtkSpd,
            item.BonusSpd
        );

        items.RemoveAt(index);

        // UI 갱신
        FindObjectOfType<TestInventoryDisplay>()?.UpdateInventoryUI(items);
        Debug.Log($"아이템 제거됨: {item.Name}, 스탯도 제거됨");
    }
}
