using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;
using ChoiceDataClass = ProjectVS.Dialogue.ChoiceData.ChoiceData;
using DialogueTextTyperClass = ProjectVS.Dialogue.TextEffect.DialogueTextTyper.DialogueTextTyper;
using ProjectVS.Dialogue.TextEffect.TextTyperBase;
using DialogueLogManagerClass = ProjectVS.Dialogue.DialogueLogManager.DialogueLogManager;
using ProjectVS.Utils.UIManager;
using ProjectVS.Utils.ObservableProperty;
using ChoiceDialogueManagerClass = ProjectVS.Dialogue.ChoiceDialogueManager.ChoiceDialogueManager;
using SpriteChangeManagerClass = ProjectVS.CharacterImages.EventSpriteChangeManager.EventSpriteChangeManager;
using ProjectVS.Manager;
using ProjectVS.Util;
using ProjectVS.Interface;
using ProjectVS.Dialogue.DialogueManagerAddons.DialogueDataService;
using EvaluatorClass = ProjectVS.Dialogue.DialogueManagerAddons.DialogueConditionEvaluator.DialogueConditionEvaluator;

namespace ProjectVS.Dialogue.DialogueManagerR
{
    public enum DialogueType
    {
        ShopEnter = 1, // tsv의 발생 시점이 1부터 시작함
        ShopEvent,
        Repeat,
        StageClear,
        CutScene, // 현재는 사용 안함
        BeforeFinalStage,
        WearCostume
    }

    // TODO: 클래스에 책임이 너무 많은 것 같아서 분리해야 될 듯
    // TODO: 세이브 관련 주석 해제

    public class DialogueManagerR : SimpleSingleton<DialogueManagerR>, IManager
    {
        [Header("각 분기별 텍스트")]
        [SerializeField] private DialogueTextTyperClass _shopEnterText;
        [SerializeField] private DialogueTextTyperClass _repeatText;
        [SerializeField] private DialogueTextTyperClass _eventText;
        [SerializeField] private DialogueTextTyperClass _stageClearText;

        private DialogueTextTyperClass _currentText;

        private DialogueDataClass _currentDialogueData;

        public DialogueDataClass CurrentDialogueData => _currentDialogueData; // 코스튬 상점에서 접근하기 위해 열어둠

        public int Priority => (int)ManagerPriority.DialogueManager;

        public bool IsDontDestroy => IsDontDestroyOnLoad;


        private int _currentDialogueIndex = 100001;


        [Header("자동 진행 설정")]
        [SerializeField] private float _autoNextDelay = 1f;

        [Header("대화 로그 매니저")]
        [SerializeField] private DialogueLogManagerClass _dialogueLogManager;

        [Header("선택지 매니저")]
        [SerializeField] private ChoiceDialogueManagerClass _choiceDialogueManager;

        [Header("일러스트 변경 매니저")]
        [SerializeField] private SpriteChangeManagerClass _spriteChangeManager;

        public ObservableProperty<bool> IsAutoMode = new(false);
        public bool IsClosed = false;


        //---
        private DialogueDataService _dataService;
        private AffinityChecker _affinityChecker;
        private EvaluatorClass _evaluator;
        //---

        protected override void Awake()
        {
            base.Awake();

            _dataService = new DialogueDataService();
            _dataService.Load("Min/Resources/DialogueData.tsv", "Min/Resources/ChoiceData.tsv");

            _affinityChecker = new();
            _evaluator = new EvaluatorClass(_dataService, _affinityChecker);

            _currentText = _shopEnterText; // 임의 초기 텍스트 설정
        }

        private void Start()
        {
            // 세이브 데이터에 따라 IsPrinted 값 변경
            // 만약 이 매니저가 데이터 로드 전 부터 존재한다면 호출 순서 변경해야 됨

            //ChangeIsPrintedBySaveData();
        }


        // 씬이 변경될 때 TMP_Text를 등록해주는 메서드
        // 상점 진입 시, 인게임 씬 진입 시 호출해서 넣어줘야 됨
        // 아니면 FindObjectOfType<T>()로 찾아서 넣어줘도 됨

        public void AssignTextWhenSceneChanged(
            DialogueTextTyperClass shopEnterText = null,
            DialogueTextTyperClass repeatText = null,
            DialogueTextTyperClass eventText = null,
            DialogueTextTyperClass stageClearText = null
            )
        {
            if (shopEnterText != null)    _shopEnterText = shopEnterText;
            if (repeatText != null)       _repeatText = repeatText;
            if (eventText != null)        _eventText = eventText;
            if (stageClearText != null)   _stageClearText = stageClearText;
        }


        private void ShowDialogue(int dialogueID, DialogueTextTyperClass text)
        {
            DialogueDataClass data = _dataService.GetDialogueByID(dialogueID);
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

            if (data.OccurTiming == 1 || data.OccurTiming == 2 || data.OccurTiming == 4 || data.OccurTiming == 6)
            {
                _dialogueLogManager.AddLogBox(data.CharacterName, data.Content);
            }

            Debug.Log($"[DialogueManager] 현재 대사: {data.ID} - {data.Content}");

        }


        // TODO: 100090 ~ 100094 이미지 대체 필터링
        // TODO: Choice가 다음에 있는지 확인
        // TODO: 책임이 너무 많아진다면 분리 고려

        public void Next()
        {
            _currentDialogueIndex = _currentDialogueData.ID;
            int nextID = _currentDialogueIndex + 1;
            DialogueDataClass nextData = _dataService.GetDialogueByID(nextID);

            if (nextData == null)
            {
                Debug.LogWarning($"[DialogueManager] 다음 대사(ID {nextID})가 존재하지 않음");

                if (!IsClosed)
                {
                    UIManager.Instance.ForceCloseTopPanel();
                    IsClosed = true;
                }

                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                return;
            }

            if (_currentDialogueIndex >= _dataService.DialogueCount + 100000) // csv가 100000 부터 시작하여 (Count + 100000) 하드코딩 됨
            {
                Debug.Log("[DialogueManager] 더 이상 대사가 없습니다");

                if (!IsClosed)
                {
                    UIManager.Instance.ForceCloseTopPanel();
                    IsClosed = true;
                }

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

                if (!IsClosed)
                {
                    UIManager.Instance.ForceCloseTopPanel();
                    IsClosed = true;
                }

                IsAutoMode.Value = false;
                _dialogueLogManager.ClearLogBox();
                return;
            }

            ChoiceDataClass choiceData = _dataService.GetChoiceByNextID(nextID);
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

        public bool CanShowDialogueByType(DialogueType type)
        {
            Debug.Log($"[DialogueManager] {type} 가능 여부: {_evaluator.CanReturnDialogue(type)}");
            return _evaluator.CanReturnDialogue(type);
        }

        public void ShowDialogueByType(DialogueType type)
        {
            var data = _evaluator.ReturnDialogue(type);
            if (data == null)
            {
                Debug.LogWarning($"[DialogueManager] {type} 조건에 맞는 대사가 없습니다.");
                return;
            }

            DialogueTextTyperClass typer = GetTextTyperByType(type);
            ShowDialogue(data.ID, typer);
        }

        private DialogueTextTyperClass GetTextTyperByType(DialogueType type)
        {
            return type switch
            {
                DialogueType.ShopEnter => _shopEnterText,
                DialogueType.ShopEvent => _eventText,
                DialogueType.Repeat => _repeatText,
                DialogueType.StageClear => _stageClearText,
                DialogueType.BeforeFinalStage => _stageClearText,
                DialogueType.WearCostume => _repeatText,
                _ => _eventText
            };
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
            Debug.Log($"[DialogueManager] 현재 AutoMode: {IsAutoMode.Value}");

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
                else
                {
                    Debug.LogWarning($"[DialogueManager] _currentText가 null입니다");
                }
            }
        }


        // Auto 키가 켜져있을 때 _autoNextDelay 초 후 자동으로 next 호출
        private IEnumerator IE_AutoNextDelay()
        {
            yield return new WaitForSecondsRealtime(_autoNextDelay);

            if (!IsAutoMode.Value)
            {
                Debug.Log("[DialogueManager] AutoMode가 중간에 꺼져서 자동 진행 중단");
                yield break;
            }

            Next();
        }


        // Auto가 켜져있을 때, 타이핑이 완료되면 자동으로 다음 대사로 넘어가게 하는 메서드
        private void HandleAutoAfterTyping()
        {
            _currentText.OnTypingComplete -= HandleAutoAfterTyping; // 한 번만 실행되도록 구독 취소
            StartCoroutine(IE_AutoNextDelay());
        }


        // 세이브할 때 읽은 대사 번호 List<int> 로 반환
        // 내가 다시 필터링할 때 해당 List 참조하여 필터링

        public HashSet<int> GetReadDialogueIDs() // 세이브 시 호출하여 반환
        {
            HashSet<int> readIDs = new();
            foreach (var data in _dataService.GetAllDialogues())
            {
                if (data.IsPrinted)
                {
                    readIDs.Add(data.ID);
                }
            }
            return readIDs;
        }

        private void ChangeIsPrintedBySaveData()
        {
            HashSet<int> savedIDs = PlayerDataManager.Instance.ReadDialogeIDs;

            if (savedIDs == null || savedIDs.Count == 0)
            {
                Debug.Log("[DialogueManager] 저장된 대사 ID가 없습니다.");
                return;
            }

            foreach (var data in _dataService.GetAllDialogues())
            {
                if (savedIDs.Contains(data.ID))
                {
                    data.IsPrinted = true;
                }
            }
        }

        public void Initialize()
        {
            //GetReadDialogueIDs();
            //ChangeIsPrintedBySaveData();
        }

        public void Cleanup() { }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
