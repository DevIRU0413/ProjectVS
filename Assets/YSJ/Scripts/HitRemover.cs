using UnityEngine;

public class HitRemover : MonoBehaviour
{
    [SerializeField] private float _delayDstory = 0.0f; // 제거 딜레이
    [SerializeField] private GameObject _vfx; // 비주얼 이펙트

    private void Awake()
    {
        Hitable hitable = GetComponent<Hitable>();
        hitable.OnEnterHitEnd -= Remove;
        hitable.OnEnterHitEnd += Remove;
    }

    private void Remove()
    {
        if (_vfx != null)
            GameObject.Instantiate(_vfx);

        Destroy(this.gameObject, _delayDstory);
    }
}
