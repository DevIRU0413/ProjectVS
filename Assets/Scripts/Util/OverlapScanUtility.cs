using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public static class OverlapScanUtility
    {
        /*/// <summary>
        /// 조건에 맞는 GameObject를 스캔 범위 내에서 찾아 리스트로 반환함
        /// </summary>
        public static List<GameObject> Scan(float radius, Vector2 center, LayerMask mask, Func<GameObject, bool> filter = null)
        {
            var results = new List<GameObject>();

            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, mask);

            for (int i = 0; i < count; i++)
            {
                GameObject go = buffer[i].gameObject;
                if (go == null || !go.activeInHierarchy)
                    continue;

                if (filter == null || filter.Invoke(go))
                    results.Add(go);
            }

            return results;
        }

        /// <summary>
        /// 가장 가까운 GameObject 하나만 반환
        /// </summary>
        public static GameObject GetNearest(float radius, Vector2 center, LayerMask mask, Func<GameObject, bool> filter = null)
        {
            float minSqr = float.MaxValue;
            GameObject nearest = null;

            int count = Physics2D.OverlapCircleNonAlloc(center, radius, _buffer, mask);

            for (int i = 0; i < count; i++)
            {
                GameObject go = _buffer[i].gameObject;
                if (go == null || !go.activeInHierarchy) continue;
                if (filter != null && !filter.Invoke(go)) continue;

                float sqrDist = ((Vector2)go.transform.position - center).sqrMagnitude;
                if (sqrDist < minSqr)
                {
                    minSqr = sqrDist;
                    nearest = go;
                }
            }

            return nearest;
        }*/
    }
}
