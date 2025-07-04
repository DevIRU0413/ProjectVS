using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;

using UnityEngine;

public class TestPlayerInventory : MonoBehaviour
{
    public int MaxItemCount = 8;

    public List<TestItemData> items = new List<TestItemData>();

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
    public void RemoveItem(TestItemData item)
    {
        items.Remove(item);
        FindObjectOfType<TestInventoryDisplay>()?.UpdateInventoryUI(items);
    }
}
