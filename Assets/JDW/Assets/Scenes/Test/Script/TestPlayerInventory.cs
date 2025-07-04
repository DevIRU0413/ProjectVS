using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.JDW;
using ProjectVS.Manager;

using UnityEngine;

using static UnityEditor.Progress;

public class TestPlayerInventory : MonoBehaviour // 아이템 ui를 위한 테스트용 인벤토리
{
    public int MaxItemCount = 8; // 최대 8개 까지의 아이템을 들고있을 수 있음

    public List<TestItemData> items = new List<TestItemData>(); // 인벤토리 내 테스트 아이템 리스트

    public PlayerConfig PlayerConfig;

    public void AddItem(TestItemData item)
    {
        if (items.Count >= MaxItemCount) // 아이템이 8개 이하라면 아이템 추가
        {
            Debug.LogWarning("인벤토리에 더 이상 아이템을 추가할 수 없습니다.");
            return;
        }
        items.Add(item);
        Debug.Log($"아이템 추가됨: {item.Name}");
        FindObjectOfType<InventoryDisplay>()?.UpdateInventoryUI(items); // Ui의 아이템의 아이콘 생성

    }
    public void DiscardItem(int index) // 인벤토리에서 특정 인덱스의 아이템을 파기 ( 리스트 제거 + 스탯 감소 + UI 반영)
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
        FindObjectOfType<InventoryDisplay>()?.UpdateInventoryUI(items);
        Debug.Log($"아이템 제거됨: {item.Name}, 스탯도 제거됨");
    }  
}
