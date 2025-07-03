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
        [SerializeField] protected TMP_Text _nameText;
        [SerializeField] protected TMP_Text _contentText;
        [SerializeField] protected float _normalTypingSpeed = 0.05f;
        [SerializeField] protected float _skippedTypingSpeed = 0.01f;
        // [SerializeField] protected float _cursorBlinkingInterval = 0.5f;

        protected Coroutine _typingCo;
        // protected Coroutine _cursorBlinkingCo;

        protected string _currentContent;

        protected WaitForSecondsRealtime normalTypingSeconds;
        protected WaitForSecondsRealtime skippedTypingSeconds;


        public Action OnTypingComplete;
        public bool IsTyping => _typingCo != null; // 현재 타이핑 중인지 확인할 수 있는 프로퍼티

        public ObservableProperty<bool> IsSkipToggled = new();

        // private const string Cursor_Text = "█";


        protected virtual void Awake()
        {
            normalTypingSeconds = new WaitForSecondsRealtime(_normalTypingSpeed);
            skippedTypingSeconds = new WaitForSecondsRealtime(_skippedTypingSpeed);
        }

        protected virtual void OnEnable()
        {
            IsSkipToggled.Value = false;
        }

        protected virtual void OnDisable()
        {
            // 비활성화되는 시점에 텍스트가 짤리지 않게 풀 텍스트 강제 대입
            if (_typingCo != null)
            {
                StopCoroutine(_typingCo);
                _contentText.text = _currentContent;
                _typingCo = null;
                OnTypingComplete?.Invoke();
            }
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

            foreach (char c in content)
            {
                sb.Append(c);
                _contentText.text = sb.ToString();

                if (IsSkipToggled.Value)
                {
                    yield return skippedTypingSeconds;
                }
                else
                {
                    yield return normalTypingSeconds;
                }
            }

            _typingCo = null;

            // StartCursorBlinking();

            OnTypingComplete?.Invoke();
        }


        //protected virtual void StartCursorBlinking()
        //{
        //    if (_cursorBlinkingCo != null)
        //    {
        //        StopCoroutine(_cursorBlinkingCo);
        //    }

        //    _cursorBlinkingCo = StartCoroutine(IE_BlinkCursor());
        //}

        //protected virtual IEnumerator IE_BlinkCursor()
        //{
        //    bool showCursor = true;
        //    while (true)
        //    {
        //        _contentText.text = _currentContent + (showCursor ? Cursor_Text : "");
        //        showCursor = !showCursor;
        //        yield return new WaitForSeconds(_cursorBlinkingInterval);
        //    }
        //}


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
