using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.InGameUI.TargetArrowUI
{
    public enum TargetType
    {
        Boss,
        NPC
    }

    public class TargetArrowUI : MonoBehaviour
    {
        [Header("타겟 설정")]
        [SerializeField] private TargetType _targetType;
        [SerializeField] private Transform _targetTransform;

        [Header("컴포넌트 참조")]
        [SerializeField] private RectTransform _arrowTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _image;

        [Header("화살표 UI 거리 설정")]
        [SerializeField] private float borderOffset = 50f; // 클 수록 플레이어와 가까워짐


        private void Awake()
        {
            if (_canvas == null)
            {
                Debug.LogWarning("[TargetArrowUI] Canvas가 등록되지 않았습니다.");
            }
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
        }

        private void Start()
        {
            _image.enabled = false;
        }

        private void Update()
        {
            TraceTargetClampedByCamera();
        }


        private void TraceTargetClampedByCamera()
        {
            if (_targetTransform == null) return;

            // 타겟의 스크린 좌표 계산
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position);

            // 화면 중앙
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2f;

            // 화면 안에 있으면 숨김 처리
            if (screenPos.z < 0 ||       // 카메라 뒤쪽
                (screenPos.x >= 0 && screenPos.x <= Screen.width &&
                 screenPos.y >= 0 && screenPos.y <= Screen.height))
            {
                _image.enabled = false;
                return;
            }

            // 클램핑 범위 설정 (UI가 모서리 바짝 붙지 않게 오프셋 적용)
            float left = borderOffset;
            float right = Screen.width - borderOffset;
            float bottom = borderOffset;
            float top = Screen.height - borderOffset;

            // 화면 바깥이라면, 위치를 경계 내로 클램핑
            Vector3 clampedScreenPos = screenPos;
            clampedScreenPos.x = Mathf.Clamp(screenPos.x, left, right);
            clampedScreenPos.y = Mathf.Clamp(screenPos.y, bottom, top);

            // 중앙 → 원래 스크린 위치 방향으로 회전 각 계산
            Vector3 dir = (screenPos - screenCenter).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _arrowTransform.rotation = Quaternion.Euler(0, 0, angle - 90f); // 위쪽 기준 보정

            _image.enabled = true;

            // 화면 좌표 → UI 로컬 좌표 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                clampedScreenPos,
                null,
                out Vector2 localPos
            );

            _arrowTransform.localPosition = localPos;
        }


        // BOSS 생성 시 호출하여 초기화
        public void SetTargetTransform(Transform target)
        {
            _targetTransform = target;
        }
    }
}

