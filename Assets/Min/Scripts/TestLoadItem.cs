using System.Collections.Generic;

using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Item;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;

using UnityEngine;

public class TestLoadItem : MonoBehaviour
{
    private CsvTable _itemTable; // 테이블을 저장할 변수 선언
    private List<ItemData> _itemDatas = new(); // 아이템 데이터를 저장할 리스트 선언

    private void Awake()
    {
        _itemTable = new CsvTable("Resources/CSV/Item.csv", '\t'); // CsvTable 객체 생성, 파일 경로와 구분자 지정, tsv로 받았습니다.
        CsvReader.Read(_itemTable); // CsvReader를 사용하여 테이블을 읽어옵니다.

        _itemDatas = ItemDataParser.Parse(_itemTable); // ItemDataParser를 사용하여 아이템 데이터를 파싱

        HowToUse();
    }

    // 사용 방법 예시
    private void HowToUse()
    {
        ItemData Data = _itemDatas.Find(d => d.MaxLevel >= 3);
        Debug.Log($"{Data.ItemID}의 MaxLevel: {Data.MaxLevel}");

        foreach (var item in _itemDatas)
        {
            if (item.ItemRank != (ItemRank)1) continue;
            Debug.Log($"{item.ItemName}의 등급: {item.ItemRank}");
        }
    }
}
