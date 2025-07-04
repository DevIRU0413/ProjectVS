using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public static class OverlapScanUtility
    {
        public static int CircleScan(Vector2 center, float radius, LayerMask mask, Collider2D[] buffer)
        {
            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, mask);
            return count;
        }

        public static int BoxScan(Vector2 center, Vector2 offset, Vector2 size, float angle, Collider2D[] buffer, LayerMask targetMask)
        {
            int count = Physics2D.OverlapBoxNonAlloc(center + offset, size, angle, buffer, targetMask);
            return count;
        }

        public static List<GameObject> CircleScan(float radius, Vector2 center, LayerMask mask, Collider2D[] buffer, Func<GameObject, bool> filter = null)
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

        public static List<GameObject> BoxScan(Vector2 center, Vector2 offset, Vector2 size, float angle, Collider2D[] buffer, LayerMask targetMask, Func<GameObject, bool> filter = null)
        {
            var results = new List<GameObject>();

            int count = BoxScan(center, offset, size, angle, buffer, targetMask);
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
