using System.Collections;
using System.Collections.Generic;

using AlertArrowUIClass = ProjectVS.UIs.InGameUI.AlertArrowUI.AlertArrowUI;
using ProjectVS.Utils.PooledObject;

using UnityEngine;
using ProjectVS.Utils.ObjectPool;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;


namespace ProjectVS.UIs.InGameUI.AlertUIManager
{
    public class AlertUIManager : MonoBehaviour
    {
        [SerializeField] private AlertArrowUIClass _alertPrefab;
        [SerializeField] private Transform _poolParent;
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private Camera _playerCamera;
        //[SerializeField] private Transform _testTarget; // 테스트용

        private ObjectPool<AlertArrowUIClass> _arrowPool;

        private void Awake()
        {
            _arrowPool = new ObjectPool<AlertArrowUIClass>(_poolParent, _alertPrefab, 5);
        }

        //private void Update()
        //{
        //    if (Keyboard.current.zKey.wasPressedThisFrame)
        //    {
        //        SpawnAlertArrow(_testTarget.position); // 테스트용
        //    }
        //}

        /// <summary>
        /// 위험 경고 UI를 스폰하는 메서드입니다.
        /// </summary>
        /// <param name="worldPosition">표시할 장소의 Vector3 입니다</param>
        /// <param name="borderX">화면 좌우 가장자리 여백 비율 (0 ~ 1). 기본값은 0.05입니다.(Optional Parameter)</param>
        /// <param name="borderY">화면 상하 가장자리 여백 비율 (0 ~ 1). 기본값은 0.08입니다.(Optional Parameter)</param>
        public void SpawnAlertArrow(Vector3 worldPosition, float borderX = 0.05f, float borderY = 0.08f)
        {
            AlertArrowUIClass arrow = _arrowPool.PopPool();
            arrow.transform.SetParent(_canvasRect, false);
            arrow.Init(worldPosition, _playerCamera, _canvasRect, borderX, borderY);
        }
    }
}
