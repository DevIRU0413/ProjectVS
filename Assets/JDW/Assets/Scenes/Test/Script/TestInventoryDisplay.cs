using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;

using UnityEngine;
using UnityEngine.UI;

public class TestInventoryDisplay : MonoBehaviour
{
    public Transform SlotParent;  // 슬롯이 들어갈 부모
    public GameObject SlotPrefab; // 슬롯 프리팹
    private List<GameObject> ActiveSlots = new(); // 현재 표시된 슬롯

    public void UpdateInventoryUI(List<TestItemData> items)
    {
        // 기존 슬롯 제거
        foreach (Transform child in SlotParent)
        {
            Destroy(child.gameObject);
        }
        // 현재 인벤토리 기준으로 슬롯 다시 생성
        foreach (var item in items)
        {
            GameObject slot = Instantiate(SlotPrefab, SlotParent);
            slot.transform.SetParent(SlotParent, false); // 명시적으로 부모 설정
            var img = slot.GetComponentInChildren<Image>(); // ← 자식까지 탐색

            if (img != null)
            {
                if (item.Icon != null)
                {
                    img.sprite = item.Icon;
                }
                else
                {
                    Debug.LogWarning($"[InventoryUI] 아이템 '{item.Name}'의 아이콘이 null임.");
                }
            }
            else
            {
                Debug.LogError($"[InventoryUI] 슬롯 프리팹에 Image 컴포넌트가 없음. {slot.name}");
            }
        }


    }
}
