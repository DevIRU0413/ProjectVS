using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.PooledObject;

using UnityEngine;


namespace ProjectVS.UIs.InGameUI.AlertArrowUI
{
    public class AlertArrowUI : PooledObject<AlertArrowUI>
    {
        [SerializeField] private RectTransform _arrowRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 2f;

        private Coroutine _blinkCo;
        private Vector3 _targetPosition;
        private Camera _cam;
        private RectTransform _canvasRect;

        private float _borderX;
        private float _borderY;

        private void Awake()
        {
            if (_arrowRect == null)
            {
                _arrowRect = GetComponent<RectTransform>();
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public void Init(Vector3 targetPosition, Camera cam, RectTransform canvasRect, float borderX, float borderY)
        {
            // 필드 초기화
            _targetPosition = targetPosition;
            _cam = cam;
            _canvasRect = canvasRect;
            _borderX = borderX;
            _borderY = borderY;

            // 회전 및 크기 초기화
            _arrowRect.localRotation = Quaternion.identity;
            _arrowRect.localScale = Vector3.one;

            // 코루틴 중복 방지
            if (_blinkCo != null)
            {
                StopCoroutine(_blinkCo);
            }

            _blinkCo = StartCoroutine(BlinkAndReturn());

            gameObject.SetActive(true);
        }

        private void Update()
        {
            TraceTargetClampedByCamera();
        }

        private void TraceTargetClampedByCamera()
        {
            // 카메라 기준으로 위치 반환
            Vector3 viewportPos = _cam.WorldToViewportPoint(_targetPosition);

            // 카메라 뒤쪽에 있으면 반환
            if (viewportPos.z < 0f)
            {
                Debug.LogWarning($"[AlertArrowUI] 오브젝트가 카메라보다 뒤에 있습니다");
                ReturnPool();
                return;
            }

            // 화면 밖인지 판단
            bool isOffScreen = viewportPos.x < 0 || viewportPos.x > 1 ||
                               viewportPos.y < 0 || viewportPos.y > 1;


            if (isOffScreen)
            {
                viewportPos.x = Mathf.Clamp(viewportPos.x, _borderX, 1f - _borderX);
                viewportPos.y = Mathf.Clamp(viewportPos.y, _borderY, 1f - _borderY);
            }


            // 뷰포트 좌표에서 실제 스크린 좌표로 변환
            Vector3 screenPos = _cam.ViewportToScreenPoint(viewportPos);
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height) / 2f;

            // 중앙 기준으로 방향 벡터 계산
            Vector3 dir = (screenPos - screenCenter).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // 화면 밖일 경우 방향 회전 적용, 아니면 그대로
            // 일단 안쓸 수도 있어서 주석처리
            // _arrowRect.rotation = isOffScreen ? Quaternion.Euler(0, 0, angle - 90f) : Quaternion.identity;

            // 화면 좌표를 캔버스 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect, screenPos, null, out Vector2 localPoint
            );

            // 위치 반영
            _arrowRect.localPosition = localPoint;
        }

        // 깜빡임 코루틴
        private IEnumerator BlinkAndReturn()
        {
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                _canvasGroup.alpha = Mathf.PingPong(Time.time * 2f, 1f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
            ReturnPool();
        }

        private void OnDisable()
        {
            if (_blinkCo != null)
                StopCoroutine(_blinkCo);

            _canvasGroup.alpha = 1f;
        }
    }   
}
