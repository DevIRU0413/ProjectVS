using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using ItemDataClass = ProjectVS.ItemData.ItemData.ItemData;
using ItemDataParserClass = ProjectVS.ItemData.ItemDataParser.ItemDataParser;
using ProjectVS.ItemData.ItemData;


public class TestLoadItem : MonoBehaviour
{
    private CsvTable _itemTable; // 테이블을 저장할 변수 선언
    private List<ItemDataClass> _itemDatas = new(); // 아이템 데이터를 저장할 리스트 선언

    private void Awake()
    {
        _itemTable = new CsvTable("Resources/CSV/Item.csv", '\t'); // CsvTable 객체 생성, 파일 경로와 구분자 지정, tsv로 받았습니다.
        CsvReader.Read(_itemTable); // CsvReader를 사용하여 테이블을 읽어옵니다.

        _itemDatas = ItemDataParserClass.Parse(_itemTable); // ItemDataParser를 사용하여 아이템 데이터를 파싱

        HowToUse();
    }


    // 사용 방법 예시
    private void HowToUse()
    {
        ItemDataClass Data = _itemDatas.Find(d => d.MaxLevel >= 3);
        Debug.Log($"{Data.ID}의 MaxLevel: {Data.MaxLevel}");

        foreach (var item in _itemDatas)
        {
            if (item.ItemRank != (ItemRank)1) continue;
            Debug.Log($"{item.ItemName}의 등급: {item.ItemRank}");
        }
    }
}
