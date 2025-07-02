using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    // 레시피(조합 법) 등록
    [Header("All Recipe")]
    [SerializeField] private List<Recipe> _recipes;

    // 조합 재료를 저장하기 위한 List
    public List<Stuff> CraftStuff { get; private set; }

    public Action OnCraft;

    private void Awake() => Init();

    private void Init()
    {
        CraftStuff = new List<Stuff>();
    }

    private void Update()
    {
        PossibleCraft();
    }

    // 조합 가능한 레시피인지?
    public Recipe PossibleCraft()
    {
        // 레시피 리스트 전체를 돌면서
        foreach (var recipe in _recipes)
        {
            int count = 0;

            // 조합이 가능한 레시피가 맞는지 돌면서
            foreach (var ingredient in recipe.ingredients)
            {
                // 만약 조합 가능한 레시피라면
                if (CraftStuff.Contains(ingredient.Stuff))
                {
                    count++;
                    Debug.Log("조합 가능");
                    OnCraft?.Invoke();
                }
            }

            // 조합 가능한 재료가 2개면
            if (count == 2)
            {
                // 레시피 반환
                return recipe;
            }
        }

        // 레시피 없으면 null 반환
        return null;
    }

    // 기존 아이템 제거 및 레시피 결과 아이템 반환
    public GameObject Craft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            CraftStuff.Remove(ingredient.Stuff);
        }
        return recipe.result;
    }

    public void AddItem(Stuff item)
    {
        CraftStuff.Add(item);
    }
}
