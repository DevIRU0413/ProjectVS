using UnityEngine;

public struct DamageInfo
{
    public float Amount;
    public float CriticalMultiplier;
    public bool IsCritical;
    public bool IsPiercing;
    public Vector2 Direction; // 날아온 방향 확인 해서 이벤트 발생하려는 의도의 방향
    public float KnockbackForce;

    public DamageInfo(float amount, Vector2 direction)
    {
        Amount = amount;
        Direction = direction;
        CriticalMultiplier = 1f;
        IsCritical = false;
        IsPiercing = false;
        KnockbackForce = 0f;
    }
}
