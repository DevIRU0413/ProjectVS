﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftResult : MonoBehaviour
{
    [Header("Image")]

    private Sprite _resultSprite;

    // PossibleCraft 가져오고
    private Crafting _crafting;

    // 조합 결과 레시피 가져오고
    private Recipe _resultRecipe;

    private void Update()
    {
        _resultRecipe = _crafting.PossibleCraft();
        if (_resultRecipe != null)
        {
            _resultSprite = _resultRecipe.resultIcon;
        }
    }

    private void GetResult()
    {
        GameObject.Instantiate(_resultRecipe.result, transform);
    }

    private void OnEnable()
    {
        _crafting.OnCraft += GetResult;
    }
}
