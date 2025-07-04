using System.Collections;
using System.Collections.Generic;

using ProjectVS.JDW;

using UnityEngine;

public class ItemEffect_Double : MonoBehaviour
{
    private float _damage;

    public void Init(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>()?.TakeDamage(_damage);
        }
    }
}
