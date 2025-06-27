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
                    // �� ������Ʈ ����
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }

                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    // �� ó�� ������ ������ : ������, Ƚ�� ���
                    // ������ ���� �߰������� �߻��ϴ� �������̱� ������ ������
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
                    // �� ������Ʈ ����
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

        // ������ �ִ�� �ö󰡸� ����
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
