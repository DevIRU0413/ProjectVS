using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    // 장비 타입 및 수치
    public ItemDataScriptableObject.ItemType type;
    public float levelData;

    public void Init(ItemDataScriptableObject data)
    {
        // Basic Set
        name = "Equipment" + data.itemId;
        transform.parent = GameManager.instance.playerController.transform;
        transform.localPosition = Vector3.zero;
        
        // Property Set
        type = data.itemType;
        levelData = data.damages[0];

        // 처음에 기어가 생성될 때, 플레이어가 가지고 있는 모든 무기들에게 기어 적용
        ApplyEquipment();
    }

    public void LevelUp(float levelData)
    {
        this.levelData = levelData;

        // 레벨 업 후 재적용
        ApplyEquipment();
    }

    private void ApplyEquipment()
    {
        switch (type)
        {
            case ItemDataScriptableObject.ItemType.Head:
                RateUp();
                break;
            case ItemDataScriptableObject.ItemType.Body:
                RateUp();
                break;
            case ItemDataScriptableObject.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // 근거리 아이템
                case 0:
                    weapon.speed = 150 + (150 * levelData);
                    break;
                // 원거리 아이템
                default:
                    weapon.speed = 0.5f * (1f - levelData);
                    break;
            }
        }
    }

    private void SpeedUp()
    {
        float speed = 3f;

        GameManager.instance.playerController._moveSpeed = speed + speed * levelData;
    }
}
