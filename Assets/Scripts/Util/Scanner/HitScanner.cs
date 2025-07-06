using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public abstract class HitScanner : MonoBehaviour
    {
        public abstract int GetScanCount(Collider2D[] buffer);
        public abstract List<GameObject> GetScanGameObjectList(Collider2D[] buffer);
    }
}
