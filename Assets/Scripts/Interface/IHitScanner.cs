using System;
using UnityEngine;

public interface IHitScanner
{
    GameObject[] Scan(Func<GameObject, bool> filter = null);
}
