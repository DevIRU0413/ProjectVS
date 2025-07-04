using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class CircleHitScanner : HitScanner
    {
        public float radius = 1f;
        public Vector2 offset = Vector2.zero;
        public LayerMask targetMask;
        public bool showGizimo = true;
        public Color gizmoColor = new Color(0, 1, 0, 0.3f);

        public override int Scan(Collider2D[] buffer)
        {
            Vector2 center = (Vector2)transform.position + offset;
            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, targetMask);

            return count;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizimo) return;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);
        }
    }
}
