using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;
using ChoiceDataClass = ProjectVS.Dialogue.ChoiceData.ChoiceData;
using DialogueDataParserClass = ProjectVS.Dialogue.DialogueDataParser.DialogueDataParser;
using ChoiceDataParserClass = ProjectVS.Dialogue.ChoiceDataParser.ChoiceDataParser;
using NPCAffinityModelClass = ProjectVS.Shop.NPCAffinityModel.NPCAffinityModel;
using DialogueTextTyperClass = ProjectVS.Dialogue.TextEffect.DialogueTextTyper.DialogueTextTyper;
using ProjectVS.Dialogue.TextEffect.TextTyperBase;
using DialogueLogManagerClass = ProjectVS.Dialogue.DialogueLogManager.DialogueLogManager;
using ProjectVS.Utils.UIManager;
using ProjectVS.Utils.ObservableProperty;
using ChoiceDialogueManagerClass = ProjectVS.Dialogue.ChoiceDialogueManager.ChoiceDialogueManager;
using SpriteChangeManagerClass = ProjectVS.CharacterImages.SpriteChangeManager.EventSpriteChangeManager;


namespace ProjectVS.Dialogue.DialogueManager
{
    // TODO: IllustPath를 참조한 이미지 로딩 시스템
    // TODO: 클래스에 책임이 너무 많은 것 같아서 분리해야 될 듯

    public class DialogueManager : MonoBehaviour
    {
        [Header("NPC 호감도 데이터")]
        [SerializeField] private NPCAffinityModelClass _npcAffinityModel; // 싱글톤으로 바꿔서 접근해야될 듯

        [Header("각 분기별 텍스트")]
        [SerializeField] private DialogueTextTyperClass _shopEnterText;
        [SerializeField] private DialogueTextTyperClass _repeatText;
        [SerializeField] private DialogueTextTyperClass _eventText;
        [SerializeField] private DialogueTextTyperClass _stageClearText;

        private DialogueTextTyperClass _currentText;

        [Header("TSV 파일 경로")]
        [SerializeField] private string _dialoguePath = "Min/Resources/DialogueData.csv";
        [SerializeField] private string _choicePath = "Min/Resources/ChoiceData.csv";

        private CsvTable _dialogueTable;
        private CsvTable _choiceTable;

        private DialogueDataClass _currentDialogueData;

        private List<DialogueDataClass> _dialogueList = new();
        private List<ChoiceDataClass> _choiceList = new();

        private int _currentDialogueIndex = 100001;

        [Header("자동 진행 설정")]
        [SerializeField] private float _autoNextDelay = 1f;

        [Header("대화 로그 매니저")]
        [SerializeField] private DialogueLogManagerClass _dialogueLogManager;

        [Header("선택지 매니저")]
        [SerializeField] private ChoiceDialogueManagerClass _choiceDialogueManager;

        [Header("일러스트 변경 매니저")]
        [SerializeField] private SpriteChangeManagerClass _spriteChangeManager;

        [Header("스테이지 이벤트 호감도 검사 통과 조건")]
        [SerializeField] private int _passAffinity = 50;

        public ObservableProperty<bool> IsAutoMode = new(false);

        private void Awake()
        {
            LoadDialogueCSV();
            LoadChoiceCSV();

            _currentText = _shopEnterText; // 임의 초기 텍스트 설정
        }

        private void LoadDialogueCSV()
        {
            _dialogueTable = new CsvTable(_dialoguePath, '\t');
            CsvReader.Read(_dialogueTable);

            _dialogueList = DialogueDataParserClass.Parse(_dialogueTable);
        }

        private void LoadChoiceCSV()
        {
            _choiceTable = new CsvTable(_choicePath, '\t');
            CsvReader.Read(_choiceTable);

            _choiceList = ChoiceDataParserClass.Parse(_choiceTable);
        }

        private void ShowDialogue(int dialogueID, DialogueTextTyperClass text)
        {
            DialogueDataClass data = _dialogueList.Find(d => d.ID == dialogueID);
            if (data == null)
            {
                Debug.LogWarning($"[DialogueManager] 해당 ID의 대사가 없습니다: {dialogueID}");
                return;
            }

            _currentDialogueData = data;
            _currentDialogueIndex = data.ID;

            if (_currentDialogueData.IsRepeatable)
            {
                _spriteChangeManager.ChangeRepeatImage(_currentDialogueData.IllustPath);
            }
            else
            {
                _spriteChangeManager.ChangeEventImage(_currentDialogueData.IllustPath, _currentDialogueData.CharacterID);
            }

            if (text == null)
            {
                text = _currentText;
            }
            else
            {
                _currentText = text;
            }

            _currentText.ClearAction();

            if (IsAutoMode.Value)
            {
                _currentText.OnTypingComplete += () =>
                {
                    StartCoroutine(IE_AutoNextDelay());
                };
            }

            StartCoroutine(IE_WaitForAnimation(_currentText, data.CharacterName, data.Content));

            data.IsPrinted = true;

            if (data.OccurTiming == 1 || data.OccurTiming == 2 || data.OccurTiming == 4)
            {
                _dialogueLogManager.AddLogBox(data.CharacterName, data.Content);
            }

            Debug.Log($"[DialogueManager] 현재 대사: {data.ID} - {data.Content}");

            // TODO: 일러스트, 선택지 UI 등 출력 처리
        }


        // TODO: 100090 ~ 100094 이미지 대체 필터링
        // TODO: Choice가 다음에 있는지 확인
        // TODO: 책임이 너무 많아진다면 분리 고려

        public void Next()
        {
            _currentDialogueIndex = _currentDialogueData.ID;
            int nextID = _currentDialogueIndex + 1;
            DialogueDataClass nextData = _dialogueList.Find(d => d.ID == nextID);

            if (nextData == null)
            {
                Debug.LogWarning($"[DialogueManager] 다음 대사(ID {nextID})가 존재하지 않음");
                UIManager.Instance.ForceCloseTopPanel();
                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                return;
            }

            if (_currentDialogueIndex >= _dialogueList.Count + 100000) // csv가 100000 부터 시작하여 (Count + 100000) 하드코딩 됨
            {
                Debug.Log("[DialogueManager] 더 이상 대사가 없습니다");
                UIManager.Instance.ForceCloseTopPanel();
                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                _currentDialogueIndex--;
                return;
            }

            if (_currentDialogueData.OccurTiming != nextData.OccurTiming)
            {
                Debug.Log($"현재 데이터 {_currentDialogueData.OccurTiming}, 다음 {nextData.OccurTiming}");
                Debug.Log($"현재 {_currentDialogueData.ID}, 다음 {nextData.ID}");
                Debug.Log("[DialogueManager] 다음 대사의 OccurTiming이 달라서 대화를 중단합니다.");
                UIManager.Instance.ForceCloseTopPanel();
                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                return;
            }

            if (!CheckAffinity())
            {
                Debug.Log($"[DialogueManager] 다음 대사는 접근할 수 없는 호감도 대화입니다");
                UIManager.Instance.ForceCloseTopPanel();
                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                _currentDialogueIndex--;
                return;
            }

            ChoiceDataClass choiceData = _choiceList.Find(d => d.NextDialogueID == nextID);
            if (choiceData != null)
            {
                _choiceDialogueManager.ShowChoiceButtons(
                    choiceData.ChoiceText1,
                    choiceData.ChoiceText2,
                    () =>
                    {
                        _dialogueLogManager.AddLogBox(choiceData.CharacterName, choiceData.ChoiceText1);
                        ShowDialogue(choiceData.NextDialogueID, null);
                    },
                    () =>
                    {
                        _dialogueLogManager.AddLogBox(choiceData.CharacterName, choiceData.ChoiceText2);
                        ShowDialogue(choiceData.NextDialogueID, null);
                    });

                return;
            }

            _currentDialogueIndex = nextID;
            ShowDialogue(nextID, null);
        }

        private bool CheckAffinity()
        {
            DialogueDataClass data = _dialogueList.Find(d => d.ID == _currentDialogueIndex);

            if (_npcAffinityModel.Affinity < data.NeedAffinity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CanShowShopEnterDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 1) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;

                return true;
            }

            return false;
        }


        [ContextMenu("Show Shop Enter Dialogue")]
        public void ShowShopEnterDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 1) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;

                ShowDialogue(data.ID, _shopEnterText);
                break;
            }
        }


        [ContextMenu("Show Event Dialogue")]
        public void ShowEventDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 2) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;

                ShowDialogue(data.ID, _eventText);
                return;
            }

            Debug.Log("[DialogueManager] 출력할 이벤트 대사가 없습니다");
        }


        // 이벤트가 실행가능한 상황인지 판단
        public bool CanShowEventDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 2) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;

                return true;
            }

            return false;
        }


        [ContextMenu("Show Repeat Dialogue")]
        public void ShowRepeatDialogue()
        {
            List<DialogueDataClass> repeatables = new();

            foreach (var data in _dialogueList)
            {
                if (!data.IsRepeatable) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;

                repeatables.Add(data);
            }

            if (repeatables.Count == 0)
            {
                Debug.Log("[DialogueManager] 반복 출력 가능한 대사가 없습니다");
                return;
            }

            // 가장 높은 NeedAffinity 값 필터
            int maxAffinity = -1;
            foreach (var data in repeatables)
            {
                if (data.NeedAffinity > maxAffinity)
                {
                    maxAffinity = data.NeedAffinity;
                }
            }

            List<DialogueDataClass> filtered = repeatables.FindAll(d => d.NeedAffinity == maxAffinity);
            DialogueDataClass selected = filtered[Random.Range(0, filtered.Count)];

            ShowDialogue(selected.ID, _repeatText);
        }


        public bool CanShowStageClearDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 4) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;
                if (HasUnshownPreviousEventDialogue(data)) continue;

                return true;
            }

            return false;
        }


        // 스테이지 종료 시 호출할 메서드
        [ContextMenu("Show Stage Clear Dialogue")]
        public void ShowStageClearDialogue()
        {
            foreach (var data in _dialogueList)
            {
                if (data.OccurTiming != 4) continue;
                if (_npcAffinityModel.Affinity < data.NeedAffinity) continue;
                if (data.IsPrinted) continue;
                if (HasUnshownPreviousEventDialogue(data)) continue;

                ShowDialogue(data.ID, _stageClearText);
                return;
            }

            Debug.Log("[DialogueManager] 스테이지 종료 후 출력할 대사 없음");
        }


        // 이전 이벤트 대사를 출력했는지 확인하는 메서드
        private bool HasUnshownPreviousEventDialogue(DialogueDataClass stageClearData)
        {
            // 첫 대사라면 안 본 대사 체크가 무의미하기에 false 반환
            if (stageClearData.ID == 100001) return false;

            // 호감도 50미만이면 이전 이벤트 대사 체크 없이 true 반환
            // TODO: 추후에 로직 바꿔야 될 수도
            if (stageClearData.NeedAffinity < _passAffinity)
            {
                return false;
            }

            return _dialogueList.Exists(data =>
                data.OccurTiming == 2 && // 이벤트 대사가 존재했는지 확인
                data.NeedAffinity < stageClearData.NeedAffinity && // 현재 스테이지 클리어 호감도보다 낮은 호감도 조건인데
                _npcAffinityModel.Affinity >= data.NeedAffinity && // 현재 호감도로 볼 수 있는 대사가 존재하는지 확인
                !data.IsPrinted // 아직 보지 않은 대사인지 확인
            );
        }

        public void ShowCutsceneDialogue()
        {
            // TODO: 컷신 대사 출력 로직 구현
        }

        // 패널의 애니메이션으로 인한 활성화 지연 대기 코루틴
        private IEnumerator IE_WaitForAnimation(TextTyperBase text, string name, string content)
        {
            yield return new WaitUntil(() => text.gameObject.activeInHierarchy);
            yield return null;

            text.StartNameTyping(name);
            text.StartContentTyping(content);
        }

        public void OnToggleAutoMode()
        {
            IsAutoMode.Value = !IsAutoMode.Value;
            Debug.Log($"현재 AutoMode: {IsAutoMode.Value}");

            // 이전 이벤트 제거
            _currentText.OnTypingComplete -= HandleAutoAfterTyping;

            if (IsAutoMode.Value)
            {
                if (_currentText != null)
                {
                    if (_currentText.IsTyping)
                    {
                        // 타이핑 중이면 끝날 때 자동 다음 진행
                        _currentText.OnTypingComplete += HandleAutoAfterTyping;
                    }
                    else
                    {
                        // 타이핑 중이 아니면 바로 진행
                        StartCoroutine(IE_AutoNextDelay());
                    }
                }
            }
        }


        // Auto 키가 켜져있을 때 _autoNextDelay 초 후 자동으로 next 호출
        private IEnumerator IE_AutoNextDelay()
        {
            yield return new WaitForSeconds(_autoNextDelay);
            Next();
        }


        // Auto가 켜져있을 때, 타이핑이 완료되면 자동으로 다음 대사로 넘어가게 하는 메서드
        private void HandleAutoAfterTyping()
        {
            _currentText.OnTypingComplete -= HandleAutoAfterTyping; // 한 번만 실행되도록 구독 취소
            StartCoroutine(IE_AutoNextDelay());
        }
    }
}
