using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    // ������(���� ��) ���
    [Header("All Recipe")]
    [SerializeField] private List<Recipe> recipes;

    // ���� ��Ḧ �����ϱ� ���� List
    [SerializeField] public List<Stuff> craftStuff;

    //private void Init()
    //{
    //    craftStuff = new List<Stuff>();
    //}

    //private void Awake() => Init();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PossibleCraft();
        }
    }

    // ���� ������ ����������?
    public Recipe PossibleCraft()
    {
        // ������ ����Ʈ ��ü�� ���鼭
        foreach (var recipe in recipes)
        {
            int count = 0;

            // ������ ������ �����ǰ� �´��� ���鼭
            foreach (var ingredient in recipe.ingredients)
            {
                // ���� ���� ������ �����Ƕ��
                if (craftStuff.Contains(ingredient.Stuff))
                {
                    count++;
                    Debug.Log("���� ����");
                }
            }

            // ���� ������ ��ᰡ 2����
            if (count == 2)
            {
                // ������ ��ȯ
                return recipe;
            }
        }

        // ������ ������ null ��ȯ
        return null;
    }

    // ���� ������ ���� �� ������ ��� ������ ��ȯ
    public Item Craft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            craftStuff.Remove(ingredient.Stuff);
        }
        return recipe.result;
    }

    /// <summary>
    /// List<Recipe>�� �����۵� �־���� ���� �Լ�
    /// </summary>
    public void AddItem()
    {
        //craftStuff.Add()
    }

}
