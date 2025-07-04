using System.Collections.Generic;
using System;
using UnityEngine;

namespace ProjectVS.Util
{
    public abstract class HitScanner : MonoBehaviour
    {
        public abstract int Scan(Collider2D[] buffer);
    }
}
