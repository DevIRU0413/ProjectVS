using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class CircleHitScanner : MonoBehaviour, IHitScanner
    {
        public float radius = 1f;
        public Vector2 offset = Vector2.zero;
        public Collider2D[] buffer = new Collider2D[20];
        public LayerMask targetMask;
        public bool showGizimo = true;
        public Color gizmoColor = new Color(0, 1, 0, 0.3f);

        public GameObject[] Scan(Func<GameObject, bool> filter = null)
        {
            Vector2 center = (Vector2)transform.position + offset;
            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, targetMask);

            var list = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                var go = buffer[i].gameObject;
                if (go != null && go.activeInHierarchy && (filter == null || filter(go)))
                    list.Add(go);
            }
            return list.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizimo) return;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);
        }
    }
}
