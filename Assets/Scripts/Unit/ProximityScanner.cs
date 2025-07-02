using System;
using System.Collections.Generic;

using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Unit
{
    public class ProximityScanner : MonoBehaviour
    {
        // 초기 세팅 값 (필수)
        [Header("Scanner Settings")]
        [SerializeField] private float scanRange = 5f;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private GameObject _scannerObject;

        // 필수
        private CircleCollider2D _sannerCollider;
        private Collider2DAction _scannerTrigger;

        // 범위 내 검출된 타겟
        private List<Transform> _targets = new();

        // 필터링 조건
        private GameObjectConditionEvaluator _condiotions = new();
        protected void AddCondition(Func<GameObject, bool> condition) => _condiotions?.Add(condition);

        // 가장 가까운 타겟
        public Transform NearestTarget { get; private set; }
        // 범위 내 전체 타겟
        public List<Transform> Targets => _targets;

        protected virtual void Awake()
        {
            if (_scannerObject == null)
            {
                _scannerObject = new GameObject("ProximityScanner");
                _scannerObject.transform.parent = this.transform;
            }

            _sannerCollider = _scannerObject.GetOrAddComponent<CircleCollider2D>();
            _scannerTrigger = _scannerObject.GetOrAddComponent<Collider2DAction>();

            _sannerCollider.isTrigger = true;
            _sannerCollider.radius = scanRange;

            _scannerTrigger.OnTriggerEnterAction += OnEnter;
            _scannerTrigger.OnTriggerExitAction += OnExit;
        }

        private void FixedUpdate()
        {
            NearestTarget = GetNearestTarget();
        }

        private void OnEnter(Collider2D col)
        {
            if (!IsInLayer(col.gameObject.layer, targetLayer)) return;

            if (_condiotions.EvaluateAll(col.gameObject))
            {
                if (!_targets.Contains(col.transform))
                    _targets.Add(col.transform);
            }
        }

        private void OnExit(Collider2D col)
        {
            _targets.Remove(col.transform);
        }

        private Transform GetNearestTarget()
        {
            if (_targets.Count == 0) return null;

            float minSqr = float.MaxValue;
            Transform closest = null;
            Vector2 origin = transform.position;

            foreach (var t in _targets)
            {
                if (t == null) continue;
                if (t.gameObject.activeSelf == false) continue;

                float sqrDist = ((Vector2)t.position - origin).sqrMagnitude;
                if (sqrDist < minSqr)
                {
                    minSqr = sqrDist;
                    closest = t;
                }
            }

            return closest;
        }

        private bool IsInLayer(int layer, LayerMask mask)
        {
            return (mask & (1 << layer)) != 0;
        }

        public void SetScanRange(float range)
        {
            scanRange = range;
            if (_sannerCollider != null)
                _sannerCollider.radius = scanRange;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, scanRange);
        }
    }
}
