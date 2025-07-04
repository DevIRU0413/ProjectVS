using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void OnRemoveItemButton()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[TestButton] Player 태그 오브젝트를 찾을 수 없습니다.");
            return;
        }

        var inventory = player.GetComponent<TestPlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("[TestButton] 플레이어에 TestPlayerInventory 컴포넌트가 없습니다.");
            return;
        }

        int lastIndex = inventory.items.Count - 1;
        if (lastIndex >= 0)
        {
            inventory.DiscardItem(lastIndex); // ⭐ 중요: 스탯, UI, 리스트까지 한꺼번에 처리
        }
        else
        {
            Debug.LogWarning("[TestButton] 제거할 아이템이 없습니다.");
        }
    }
}

