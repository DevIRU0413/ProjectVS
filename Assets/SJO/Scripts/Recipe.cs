using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/RecipeData")]
public class Recipe : ScriptableObject
{
    // ���տ� �ʿ��� ��� ����Ʈ
    public List<Ingredient> ingredients;

    // ���� ���
    public Item result;

    // ���� ��� ������ Sprite
    public Sprite resultIcon;

    [System.Serializable]
    public class Ingredient
    {
        // ���� ��ũ���ͺ� ������Ʈ�� ����� ���� ���� �� �ֵ��� �ϴ� ��
        public Stuff Stuff;
        public float amount;
    }
}
