using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using ProjectVS.UIs.PanelBehaviours.CharacterIndicator;

using UnityEngine;
using UnityEngine.UI;

using CharacterIndicatorClass = ProjectVS.UIs.PanelBehaviours.CharacterIndicator.CharacterIndicator;


namespace ProjectVS.UIs.CharacterSelect.SelectionEffect
{
    public class SelectionEffect : MonoBehaviour
    {
        [SerializeField] private CharacterIndicatorClass _characterIndicator;

        [SerializeField, Header("캐릭터 이미지 리스트")] private List<Image> _characterImages;

        [Header("슬롯 위치 설정")]
        [SerializeField] private RectTransform _leftSlot;
        [SerializeField] private RectTransform _centerSlot;
        [SerializeField] private RectTransform _rightSlot;

        [Header("애니메이션 설정")]
        [SerializeField] private float _moveDuration = 0.5f;
        [SerializeField] private float _curveHeight = 50f;


        // TODO
        // 현재 선택된 인덱스의 캐릭터의 정보를 읽어오기
        // List<캐릭터의 정보>로 인스펙터에 연결하고 인덱스와 동기화하면 될 듯?


        private int currentIndex = 0;
        private bool _isMoving = false;
        private Coroutine _moveCo;

        private void OnEnable()
        {
            _characterIndicator.SetActive(true);
        }

        private void OnDisable()
        {
            if (_characterIndicator != null)
            {
                _characterIndicator.SetActive(false);
            }
        }

        private void Start()
        {
            // 애니메이션 없이 즉시 업데이트
            UpdatePositions(0f);
        }

        private Vector3[] GetCurvedPath(Vector3 from, Vector3 to)
        {
            Vector3 mid = (from + to) / 2f;
            mid.y += _curveHeight; // 곡선의 높이
            return new Vector3[] { from, mid, to };
        }

        public void MoveRight()
        {
            if (_isMoving) return;

            currentIndex = (currentIndex + 1) % _characterImages.Count;
            _characterIndicator.ChangeIndex(currentIndex);
            _moveCo = StartCoroutine(AnimateMove());
        }

        public void MoveLeft()
        {
            if (_isMoving) return;

            currentIndex = (currentIndex - 1 + _characterImages.Count) % _characterImages.Count;
            _characterIndicator.ChangeIndex(currentIndex);
            _moveCo = StartCoroutine(AnimateMove());
        }

        private void UpdatePositions(float tweenDuration)
        {
            for (int i = 0; i < _characterImages.Count; i++)
            {
                Image image = _characterImages[i];
                RectTransform rect = image.rectTransform;

                int offset = (i - currentIndex + _characterImages.Count) % _characterImages.Count;

                Vector3 target;
                float scale;
                Color color;

                if (offset == 0)
                {
                    target = _centerSlot.anchoredPosition;
                    scale = 1f;
                    color = Color.white;
                }
                else if (offset == 1 || offset == _characterImages.Count - 1)
                {
                    target = offset == 1 ? _rightSlot.anchoredPosition : _leftSlot.anchoredPosition;
                    scale = 0.8f;
                    color = new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    target = new Vector3(1000, 1000, 0);
                    scale = 0.5f;
                    color = new Color(1f, 1f, 1f, 0f);
                }

                if (tweenDuration <= 0f)
                {
                    rect.anchoredPosition = target;
                    rect.localScale = Vector3.one * scale;
                    image.color = color;
                }
                else
                {
                    Vector3[] path = GetCurvedPath(rect.anchoredPosition, target);
                    rect.DOLocalPath(path, tweenDuration, PathType.CatmullRom)
                        .SetEase(Ease.InOutSine);
                    rect.DOScale(scale, tweenDuration).SetEase(Ease.InOutSine);
                    image.DOColor(color, tweenDuration);
                }
            }
        }

        private IEnumerator AnimateMove()
        {
            _isMoving = true;
            UpdatePositions(_moveDuration);
            yield return new WaitForSeconds(_moveDuration);
            _isMoving = false;
        }
    }
}
