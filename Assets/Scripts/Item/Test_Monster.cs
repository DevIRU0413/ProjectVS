using UnityEngine;

public class Test_Monster : MonoBehaviour
{
    public float maxHp = 100;
    public float currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"몬스터 피격 : {damage}, 남은 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("몬스터 사망");

        Destroy(gameObject);
    }
}
