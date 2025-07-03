using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public static class OverlapScanUtility
    {
        public static int SimpleScan(Vector2 center, float radius, LayerMask mask, Collider2D[] buffer)
        {
            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, mask);
            return count;
        }

        public static List<GameObject> Scan(float radius, Vector2 center, LayerMask mask, Collider2D[] buffer, Func<GameObject, bool> filter = null)
        {
            var results = new List<GameObject>();

            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, mask);

            for (int i = 0; i < count; i++)
            {
                var go = buffer[i].gameObject;
                if (go == null || !go.activeInHierarchy) continue;
                if (filter == null || filter(go)) results.Add(go);
            }

            return results;
        }
    }
}
