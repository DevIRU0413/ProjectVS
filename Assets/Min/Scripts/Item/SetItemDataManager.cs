using System.Collections;
using System.Collections.Generic;

using ProjectVS.Item.SetItemData;
using SetItemDataParserClass = ProjectVS.Item.SetItemDataParser.SetItemDataParser;
using ProjectVS.Util;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;

using UnityEngine;
using System.Linq;

public class SetItemDataManager : SimpleSingleton<SetItemDataManager>
{
    [SerializeField] private string _setDataPath = "Min/Resources/SetData.tsv";
    CsvTable _table;

    private List<SetItemData> _setItems;
    private Dictionary<int, SetItemData> _setItemDict;

    protected override void Awake()
    {
        base.Awake();
        LoadSetItemData();
    }

    private void LoadSetItemData()
    {
        _table = new CsvTable(_setDataPath, '\t');
        CsvReader.Read(_table);

        _setItems = SetItemDataParserClass.Parse(_table);

        _setItemDict = new();

        foreach (var set in _setItems)
        {
            if (!string.IsNullOrEmpty(set.SetItemICON))
            {
                set.SetIcon = Resources.Load<Sprite>(set.SetItemICON);

                if (set.SetIcon == null)
                    Debug.LogWarning($"[SetItemDataManager] 아이콘 로드 실패: {set.SetItemICON}");
            }

            _setItemDict[set.ID] = set;
        }

        Debug.Log($"[SetItemDataManager] Loaded {_setItems.Count} set items.");
    }

    public SetItemData GetSetItem(int id)
    {
        return _setItemDict.TryGetValue(id, out var result) ? result : null;
    }

    public List<SetItemData> GetAll() => _setItems;

    public SetItemData FindByItemID(int itemID)
    {
        foreach (var set in _setItemDict.Values)
        {
            if (set.SetItemID.Contains(itemID))
                return set;
        }

        return null;
    }
}
