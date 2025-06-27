using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemDataScriptableObject data;
    public int level;
    public Weapon weapon;
    public Equipment gear;

    public Image icon;
    public Text textLevel;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemSprite;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
    }

    private void LateUpdate()
    {
        textLevel.text = "Lv." + (level + 1);
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemDataScriptableObject.ItemType.Melee:
            case ItemDataScriptableObject.ItemType.Range:
                if (level == 0)
                {
                    // 빈 오브젝트 생성
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }

                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    // 맨 처음 이후의 레벨업 : 데미지, 횟수 계산
                    // 레벨업 이후 추가적으로 발생하는 데미지이기 때문에 더해줌
                    nextDamage += data.baseDamage * data.damages[level];

                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                break;
            case ItemDataScriptableObject.ItemType.Head:
            case ItemDataScriptableObject.ItemType.Body:
            case ItemDataScriptableObject.ItemType.Shoe:
                if (level == 0)
                {
                    // 빈 오브젝트 생성
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Equipment>();
                    gear.Init(data);
                }

                else
                {
                    float nextLevelData = data.damages[level];
                    gear.LevelUp(nextLevelData);
                }
                    break;
        }

        level++;

        // 레벨이 최대로 올라가면 막기
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
