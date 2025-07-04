using System.Collections;
using System.Collections.Generic;

using ProjectVS.JDW;

using UnityEngine;

public class ItemEffect_Double : MonoBehaviour
{
    private float _damage;

    private Test_Projectile _prj;

    public void Init(float damage)
    {
        _damage = damage;

        _prj = GetComponent<Test_Projectile>();
        _prj.InitDouble();
    }
}
