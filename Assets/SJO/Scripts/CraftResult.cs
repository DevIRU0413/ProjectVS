using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftResult : MonoBehaviour
{
    [Header("Image")]

    [SerializeField] Sprite _resultSprite;

    // PossibleCraft ��������
    private Crafting _crafting;

    // ���� ��� ������ ��������
    private Recipe _resultRecipe;

    private void Update()
    {
        _resultRecipe = _crafting.PossibleCraft();
        if (_resultRecipe != null)
        {
            _resultSprite = _resultRecipe.resultIcon;
        }
    }
}
