using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Text;

using UnityEngine;


namespace ProjectVS.Dialogue.TextEffect.TextTyperBase
{
    public class TextTyperBase : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected float _typingSpeed = 0.05f;

        protected Coroutine _typingCo;
        protected string _currentContent;

        public virtual void StartTyping(string content)
        {
            if (_typingCo != null)
            {
                StopCoroutine(_typingCo);
            }

            _currentContent = content;
            _typingCo = StartCoroutine(IE_Type(content));
        }


        public virtual void Skip()
        {
            if (_typingCo != null)
            {
                StopCoroutine(_typingCo);
                _typingCo = null;
                _text.text = _currentContent;
            }
        }

        protected virtual IEnumerator IE_Type(string content)
        {
            _text.text = "";
            StringBuilder sb = new StringBuilder();

            foreach (char c in content)
            {
                sb.Append(c);
                _text.text = sb.ToString();
                yield return new WaitForSeconds(_typingSpeed);
            }

            _typingCo = null;
        }
    }
}
