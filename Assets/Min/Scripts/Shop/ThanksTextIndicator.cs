using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;


namespace ProjectVS.Shop.ThanksTextIndicator
{
    public class ThanksTextIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [Header("텍스트 설정")]
        [SerializeField] private List<string> _thanksTexts;
        [SerializeField] private float _indicateTime = 5f;

        private string _nullText = "";
        private Coroutine _indicateCO;

        private void Awake()
        {
            if (_text == null)
            {
                _text = GetComponentInChildren<TMP_Text>();
            }
        }

        [ContextMenu("Show Thanks Text")]
        public void ShowThanksText()
        {
            int randomIndex = Random.Range(0, _thanksTexts.Count);

            if (_indicateCO != null)
            {
                StopCoroutine(_indicateCO);
                _indicateCO = null;
            }

            _indicateCO = StartCoroutine(IE_Indicate(randomIndex));
        }

        private IEnumerator IE_Indicate(int value)
        {
            _text.text = _thanksTexts[value];
            yield return new WaitForSeconds(_indicateTime);
            _text.text = _nullText;

            _indicateCO = null;
        }
    }
}

