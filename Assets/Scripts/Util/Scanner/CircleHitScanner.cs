using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class CircleHitScanner : HitScanner
    {
        public float radius = 1f;
        public List<Vector2> uncheckRadiusRange = new();

        public Vector2 offset = Vector2.zero;
        public LayerMask targetMask;
        public bool showGizimo = true;
        public Color gizmoColor = new Color(0, 1, 0, 0.3f);

        public Func<GameObject, bool> filter;
        public GameObjectConditionEvaluator conditionEvaluator;

        private void Awake()
        {
            filter -= FilterCheckUnRadiusRange;
            filter += FilterCheckUnRadiusRange;

            // 작은 거 앞으로, 큰거 뒤로
            for (int i = 0; i < uncheckRadiusRange.Count; i++)
            {
                Vector2 check = uncheckRadiusRange[i];
                if (check.x > check.y)
                {
                    float temp = check.y;
                    check.y = check.x;
                    check.x = temp;

                    check.x = (check.x < 0) ? 0 : check.x;
                    check.y = (check.y < 0) ? 0 : check.y;
                    uncheckRadiusRange[i] = check;
                }

                // 값이 간다면 제거
                if (check.x == check.y)
                {
                    uncheckRadiusRange.RemoveAt(i);
                    i--;
                }
            }
        }

        public override int GetScanCount(Collider2D[] buffer)
        {
            Vector2 center = (Vector2)transform.position + offset;
            int count = OverlapScanUtility.CircleScan(center, radius, targetMask, buffer);
            return count;
        }

        public override List<GameObject> GetScanGameObjectList(Collider2D[] buffer)
        {
            Vector2 center = (Vector2)transform.position + offset;
            List<GameObject> goLIst = OverlapScanUtility.CircleScan(center, radius, targetMask, buffer, filter);
            return goLIst;
        }

        private bool FilterCheckUnRadiusRange(GameObject go)
        {
            // 센터 기준으로 들어온 옵젝의 거리 판단
            Vector2 center = (Vector2)transform.position + offset;
            float mag = ((Vector2)go.transform.position - center).magnitude;
            for (int i = 0; i < uncheckRadiusRange.Count; i++)
            {
                Vector2 check = uncheckRadiusRange[i];
                if (check.x <= mag && mag <= check.y)
                    return false;
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizimo) return;
            Vector2 center = (Vector2)transform.position + offset;

            // 전체 스캔 범위 표시
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(center, radius);

            // 제외 범위 시각화 (빨간색 테두리)
            Gizmos.color = Color.red;
            foreach (var range in uncheckRadiusRange)
            {
                // x와 y가 같은 경우는 이미 Awake에서 제거했으므로 신경 안 써도 됨
                Gizmos.DrawWireSphere(center, range.x);
                Gizmos.DrawWireSphere(center, range.y);
            }
        }
    }
}
