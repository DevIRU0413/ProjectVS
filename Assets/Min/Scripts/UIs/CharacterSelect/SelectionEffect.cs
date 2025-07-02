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
        [SerializeField] private float _mainCharacterScale = 3f;
        [SerializeField] private float _subCharacterScale = 2f;

        [Header("캐릭터 설명창, 왼쪽부터 등록")]
        [SerializeField] private List<GameObject> _characterDescriptionList;


        private int currentIndex = 0;

        private bool _isMoving = false;
        private Coroutine _moveCo;



        private void OnEnable()
        {
            _characterIndicator.SetActive(true);
            _characterIndicator.ChangeIndex(currentIndex);
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
            UpdateCharacterDescriptionEnable();
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
                    scale = _mainCharacterScale;
                    color = Color.white;
                }
                else if (offset == 1 || offset == _characterImages.Count - 1)
                {
                    target = offset == 1 ? _rightSlot.anchoredPosition : _leftSlot.anchoredPosition;
                    scale = _subCharacterScale;
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
            SetDisableAllTexts();
            UpdatePositions(_moveDuration);
            yield return new WaitForSeconds(_moveDuration);
            _isMoving = false;

            UpdateCharacterDescriptionEnable();
        }

        private void UpdateCharacterDescriptionEnable()
        {
            for (int i = 0; i < _characterImages.Count; i++)
            {
                int offset = (i - currentIndex + _characterImages.Count) % _characterImages.Count;

                if (offset == 0)
                {
                    _characterDescriptionList[i].SetActive(true);
                }
                else
                {
                    _characterDescriptionList[i].SetActive(false);
                }
            }
        }

        private void SetDisableAllTexts()
        {
            foreach (var description in _characterDescriptionList)
            {
                description.SetActive(false);
            }
        }
    }
}
