using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using CharacterSelectionDataClass = ProjectVS.CharacterSelectionData.CharacterSelectionData.CharacterSelectionData;
using ProjectVS.CharacterSelectionData.CharacterSelectionDataParser;
using ProjectVS.Utils.CsvTable;
using ProjectVS.Utils.CsvReader;
using System.Text;

namespace ProjectVS.UIs.CharacterSelect.CharacterDescriptionTextInputManager
{
    public class CharacterDescriptionTextInputManager : MonoBehaviour
    {
        [Header("TSV 파일 경로")]
        [SerializeField] private string _characterSelectionDataPath = "Min/Resources/CharacterSelectionData.tsv";

        private CsvTable _characterSelectionDataTable;

        [Header("캐릭터 별 텍스트, 왼쪽부터 등록")]
        [SerializeField] private List<TMP_Text> _characterTextsList; // left - center - right 순서로 넣으면 됨


        private StringBuilder _sb = new();
        private List<CharacterSelectionDataClass> _characterSelectionList;


        private void Awake()
        {
            LoadCharacterSelectionDataTSV();
            SetCharacterDescriptionText();
        }

        private void LoadCharacterSelectionDataTSV()
        {
            _characterSelectionDataTable = new CsvTable(_characterSelectionDataPath, '\t');
            CsvReader.Read(_characterSelectionDataTable);

            _characterSelectionList = CharacterSelectionDataParser.Parse(_characterSelectionDataTable);
        }

        private void SetCharacterDescriptionText()
        {
            for (int i = 0; i < _characterTextsList.Count; i++)
            {
                _sb.Clear();

                _sb.AppendLine($"캐릭터 이름: {_characterSelectionList[i].Name}");
                _sb.AppendLine($"고유 아이템: {_characterSelectionList[i].ItemName}");
                _sb.AppendLine($"공격력: {_characterSelectionList[i].Attack}");
                _sb.AppendLine($"방어력: {_characterSelectionList[i].Defense}");
                _sb.AppendLine($"체력: {_characterSelectionList[i].HP}");
                _sb.AppendLine($"공격 속도: {_characterSelectionList[i].AttackSpeed}");
                _sb.AppendLine($"이동 속도: {_characterSelectionList[i].MoveSpeed}");
                _sb.AppendLine($"설명: {_characterSelectionList[i].FlavorText}");

                _characterTextsList[i].text = _sb.ToString();
            }
        }
    }
}
