using System.Collections;
using System.Collections.Generic;

using UnityEngine;



namespace ProjectVS.Dialogue.DialogueData
{
    [System.Serializable]
    public class DialogueData
    {
        public int ID;
        public int CharacterID;
        public int NeedAffinity;
        public string IllustPath;
        public string Content;
        public string ContextNote;
        public int OccurTiming; // 1. 상점 진입 시, 2. 대화 선택 이후 이벤트 선택 시, 3. 대화 선택 시 반복 텍스트, 4. 스테이지 종료 시 특정 호감도 달성 시, 5. 컷씬용 텍스트
        public bool IsRepeatable => OccurTiming == 3 ? true : false;
        public bool IsPrinted = false; // 대화가 출력되었는지 여부

        public string CharacterName
        {
            get
            {
                switch (CharacterID)
                {
                    case 1:
                        return "주인공";
                    case 2:
                        return "상점 주인";
                    default:
                        return "N/A";
                }
            }
        }
    }
}
