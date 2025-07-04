using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Text;
using UnityEngine;

using ProjectVS.Utils.ObservableProperty;


namespace ProjectVS.Dialogue.TextEffect.TextTyperBase
{
    public class TextTyperBase : MonoBehaviour
    {
        [Header("텍스트 등록")]
        [SerializeField] protected TMP_Text _nameText;
        [SerializeField] protected TMP_Text _contentText;

        // 타이핑 효과
        protected float _normalTypingSpeed = 0.05f;
        protected float _skippedTypingSpeed = 0.01f;
        protected float _cursorBlinkingInterval = 0.5f;
        private const string Cursor_Text = "|";

        // 타이핑 코루틴
        protected Coroutine _typingCo;
        protected Coroutine _cursorBlinkingCo;
        protected WaitForSecondsRealtime normalTypingSeconds;
        protected WaitForSecondsRealtime skippedTypingSeconds;

        protected string _currentContent;                          // 현재 출력 중인 전체 텍스트
        private string _currentDisplayText = "";                   // 타이핑 중 보여줄 텍스트
        private bool _isTypingNow = false;                         // 타이핑 중 여부
        private bool _showCursor = true;                           // 커서 on/off 상태

        public Action OnTypingComplete;                            // 타이핑 완료 시 콜백
        public bool IsTyping => _typingCo != null;                 // 현재 타이핑 중인지 확인할 수 있는 프로퍼티

        public ObservableProperty<bool> IsSkipToggled = new();     // 스킵 토글 상태


        protected virtual void Awake()
        {
            normalTypingSeconds = new WaitForSecondsRealtime(_normalTypingSpeed);
            skippedTypingSeconds = new WaitForSecondsRealtime(_skippedTypingSpeed);
        }

        protected virtual void OnEnable()
        {
            IsSkipToggled.Value = false;
            _showCursor = true;

            if (!_isTypingNow && !string.IsNullOrEmpty(_currentContent))
            {
                StartCursorBlinking(); // 로그 패널에서 복귀 시 커서 깜빡임 재시작
            }
        }

        protected virtual void OnDisable()
        {
            if (_typingCo != null)
            {
                StopCoroutine(_typingCo);
                _typingCo = null;
            }

            if (_cursorBlinkingCo != null)
            {
                StopCoroutine(_cursorBlinkingCo);
                _cursorBlinkingCo = null;
            }

            _showCursor = true;
            _contentText.text = _currentContent; // 비활성화되는 시점에 텍스트가 짤리지 않게 풀 텍스트 강제 대입
        }

        public virtual void StartContentTyping(string content)
        {
            if (_typingCo != null)
            {
                StopCoroutine(_typingCo);
            }

            _currentContent = content;
            _typingCo = StartCoroutine(IE_Type(content));
        }

        public virtual void StartNameTyping(string name)
        {
            if (_nameText != null)
            {
                _nameText.text = name;
            }
        }

        public virtual void Skip()
        {
            IsSkipToggled.Value = !IsSkipToggled.Value;
            Debug.Log($"스킵버튼 토글 상태: {IsSkipToggled.Value}");
        }

        protected virtual IEnumerator IE_Type(string content)
        {
            _contentText.text = "";
            StringBuilder sb = new StringBuilder();

            _isTypingNow = true;

            foreach (char c in content)
            {
                sb.Append(c);
                _currentDisplayText = sb.ToString();
                UpdateTextWithCursor();

                if (IsSkipToggled.Value)
                {
                    yield return skippedTypingSeconds;
                }
                else
                {
                    yield return normalTypingSeconds;
                }
            }

            _isTypingNow = false;
            _typingCo = null;

            StartCursorBlinking();

            OnTypingComplete?.Invoke();
        }


        protected virtual void StartCursorBlinking()
        {
            if (_cursorBlinkingCo != null)
            {
                StopCoroutine(_cursorBlinkingCo);
            }

            _cursorBlinkingCo = StartCoroutine(IE_BlinkCursor());
        }

        protected virtual IEnumerator IE_BlinkCursor()
        {
            while (true)
            {
                _showCursor = !_showCursor;

                UpdateTextWithCursor();

                yield return new WaitForSecondsRealtime(_cursorBlinkingInterval);
            }
        }

        private void UpdateTextWithCursor()
        {
            if (_contentText == null) return;

            // 타이핑 중일 때는 _currentDisplayText 사용
            string baseText = _isTypingNow ? _currentDisplayText : _currentContent;

            _contentText.text = baseText + (_showCursor ? Cursor_Text : "");
        }


        public virtual void ClearAction()
        {
            OnTypingComplete = null;
        }

        // 이전 스킵용 코드

        //public virtual void Skip()
        //{
        //    if (_typingCo != null)
        //    {
        //        StopCoroutine(_typingCo);
        //        _typingCo = null;
        //        _text.text = _currentContent;
        //    }
        //}
    }
}
