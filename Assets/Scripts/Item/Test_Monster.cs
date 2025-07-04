using DG.Tweening;

using TMPro;

using UnityEngine;

public class Test_Monster : MonoBehaviour
{
    [SerializeField] TMP_Text _damageText;

    public float maxHp = 100;
    public float currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        ShowDamageText(damage);

        currentHp -= damage;
        Debug.Log($"몬스터 피격 : {damage}, 남은 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void ShowDamageText(float damage)
    {
        _damageText.text = damage.ToString();
        _damageText.alpha = 1f;
        _damageText.rectTransform.anchoredPosition = Vector2.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(_damageText.rectTransform.DOAnchorPosY(50f, 0.5f)) // anchoredPosition 기준으로 위로 이동
           .Join(_damageText.DOFade(0, 0.5f))
           .OnComplete(() =>
           {
               _damageText.alpha = 0;
               _damageText.rectTransform.anchoredPosition = Vector2.zero;
           });
    }

    private void Die()
    {
        Debug.Log("몬스터 사망");

        Destroy(gameObject);
    }
}
