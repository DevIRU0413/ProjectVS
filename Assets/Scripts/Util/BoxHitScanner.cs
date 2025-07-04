using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class BoxHitScanner : HitScanner
    {
        public Vector2 size = Vector2.one;
        public Vector2 offset = Vector2.zero;
        public Collider2D[] buffer = new Collider2D[20];
        public LayerMask targetMask;
        public bool showGizmo = true;
        public Color gizmoColor = new Color(1, 0, 0, 0.3f);

        public override int Scan(Collider2D[] buffer)
        {
            Vector2 center = transform.position;
            float angle = transform.eulerAngles.z;
            int count = OverlapScanUtility.BoxScan(center, offset, size, angle, buffer, targetMask);
            return count;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmo) return;
            Gizmos.color = gizmoColor;
            var rot = Quaternion.Euler(0, 0, transform.eulerAngles.z);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, rot, Vector3.one);
            Gizmos.DrawCube(offset, size);
        }
    }
}
