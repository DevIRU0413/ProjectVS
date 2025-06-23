using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using ProjectVS.Utils.ReferenceRegistry;


namespace ProjectVS.Utils.ReferenceProvider
{
    //[DisallowMultipleComponent]
    //public class ReferenceProvider : MonoBehaviour
    //{
    //    [SerializeField] private Component _component;

    //    private void Awake()
    //    {
    //        if (_component == null)
    //        {
    //            _component = GetComponent<IDamageable>() as Component;
    //            _component = GetComponent<Component>() as Component;
    //        }

    //        ReferenceRegistry.Register(this);
    //    }
    //    private void OnDestroy() => ReferenceRegistry.Unregister(this);

    //    public T GetAs<T>() where T : class
    //    {
    //        return _component as T;
    //    }
    //}
}
