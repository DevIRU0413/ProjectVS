using ProjectVS;
using ProjectVS.Manager;

using UnityEngine;

public class Boss : MonoBehaviour
{
    private Timer _timer;

    public float maxHp = 500;
    public float currentHp;

    [SerializeField] private int _exp = 90;
    [SerializeField] private int _gold = 500;
    [SerializeField] private GameObject _bossHpBar;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
    }
    private void Start()
    {
        currentHp = maxHp;
        _timer.PauseTimer();
        _timer.SetMessage("BOSS!");
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"보스 몬스터 피격 : {damage}, 남은 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerConfig player = playerObj.GetComponent<PlayerConfig>();
            if (player != null)
            {
                player.ExpUp(_exp);
                PlayerDataManager.Instance.gold += _gold;
                Debug.Log($"골드 흭득 : {_gold}, 현재 골드 : {player.PlayerDataManager.gold}");
                _timer.ResumeTimer();
                _timer.SetMessage("");
            }
        }
        Debug.Log("보스 몬스터 사망");

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerConfig player = other.GetComponent<PlayerConfig>();
            if (player != null)
            {
                player.TakeDamage(5);
            }
        }
    }
    private void OnEnable()
    {
        if (_bossHpBar != null)
            _bossHpBar.SetActive(true);
    }
    private void OnDisable()
    {
        if (_bossHpBar != null)
            _bossHpBar.SetActive(false);
    }
}
