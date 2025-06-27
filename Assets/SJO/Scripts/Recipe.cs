using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/RecipeData")]
public class Recipe : ScriptableObject
{
    // 조합에 필요한 재료 리스트
    public List<Ingredient> ingredients;

    // 조합 결과
    public Item result;

    // 조합 결과 아이템 Sprite
    public Sprite resultIcon;

    [System.Serializable]
    public class Ingredient
    {
        // 물건 스크립터블 오브젝트와 재료의 양을 정할 수 있도록 하는 값
        public Stuff Stuff;
        public float amount;
    }
}
