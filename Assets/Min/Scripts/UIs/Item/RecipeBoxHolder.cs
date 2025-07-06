using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RecipeBoxBehaviourClass = ProjectVS.UIs.Item.RecipeBoxBehaviour.RecipeBoxBehaviour;
using ProjectVS.Item;
using ProjectVS.Item.SetItemData;
using ProjectVS.UIs.Item.SetOrRecipeBoxBehaviour;
using TMPro;
using CsvParseUtilsClass = ProjectVS.Utils.CsvParseUtils.CsvParseUtils;


namespace ProjectVS.UIs.Item.RecipeBoxHolder
{
    public class RecipeBoxHolder : MonoBehaviour
    {
        [SerializeField] private GameObject _rightItemPanel;
        [SerializeField] private TMP_Text _rightNameText;
        [SerializeField] private TMP_Text _rightDescText;
        [SerializeField] private Image _rightItemIcon;

        [SerializeField] private GameObject _rightSetPanel;
        [SerializeField] private List<Image> _rightSetIcons;
        [SerializeField] private TMP_Text _setNameText;
        [SerializeField] private TMP_Text _setDescText;

        [SerializeField] private GameObject _leftRecipeBoxPrefab;
        [SerializeField] private Transform _recipeBoxSpawnParent;

        [SerializeField] private GameObject _leftSetBoxPrefab;
        [SerializeField] private Transform _setBoxSpawnParent;

        private List<ItemData> _recipeItems;

        private void OnEnable()
        {
            _rightItemPanel.SetActive(false);
            _rightSetPanel.SetActive(false);
        }

        private void Start()
        {
            _recipeItems = ItemDatabase.Instance.GetAllItems();

            CreateRecipeBox();
            CreateSetBox();
        }

        private void CreateRecipeBox()
        {
            foreach (var item in _recipeItems)
            {
                // 조합식이 있는 상위 아이템만 필터링
                if (item.ItemAddID1 < 0 || item.ItemAddID2 < 0)
                    continue;

                GameObject go = Instantiate(_leftRecipeBoxPrefab, _recipeBoxSpawnParent);
                RecipeBoxBehaviourClass box = go.GetComponent<RecipeBoxBehaviourClass>();
                box.Init(item, this);
            }
        }

        private void CreateSetBox()
        {
            List<SetItemData> setItems = SetItemDataManager.Instance.GetAll();

            foreach (var set in setItems)
            {
                GameObject go = Instantiate(_leftSetBoxPrefab, _setBoxSpawnParent);
                SetBoxBehaviour box = go.GetComponent<SetBoxBehaviour>();
                box.Init(set, this);
            }
        }

        public void OpenRightItemPanel(ItemData data)
        {
            _rightItemPanel.SetActive(true);
            _rightSetPanel.SetActive(false);

            _rightNameText.text = data.ItemName;
            _rightDescText.text = data.Description;
            _rightItemIcon.sprite = data.ItemIcon;
        }

        public void OpenSetPanel(SetItemData data)
        {
            _rightItemPanel.SetActive(false);
            _rightSetPanel.SetActive(true);

            _setNameText.text = data.SetName;
            _setDescText.text = CsvParseUtilsClass.ReplaceSetValuePlaceholders(data.SetEffectText, data.SetValue).Replace("\\n", "\n");

            for (int i = 0; i < _rightSetIcons.Count; i++)
            {
                if (i < data.SetItemID.Length)
                {
                    ItemData item = ItemDatabase.Instance.GetItem(data.SetItemID[i]);
                    _rightSetIcons[i].sprite = item.ItemIcon;
                    _rightSetIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    _rightSetIcons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
