using System.Collections;

using ProjectVS;
using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPosition : MonoBehaviour
{
    public Transform Player;
    public float Radius = 1.5f;
    public Vector3 Direction => _direction;
    [SerializeField] private GameObject _swordPerfab;
    [SerializeField] private GameObject _axePerfab;
    [SerializeField] private GameObject _bulletPerfab;

    [SerializeField] private Transform _muzzlePos;
    [SerializeField] private float _bulletTime = 3f;
    [SerializeField] private float _meleeAttack = 2; // 오브젝트의 잔존시간

    private Vector3 _direction;

    private Coroutine _currentRoutine;
    private PlayerConfig _playerScript;
    private PlayerStats _playerStats;

    private void Awake()
    {
        if (Player != null)
        {
            _playerScript = Player.GetComponent<PlayerConfig>();
            _playerStats = PlayerDataManager.ForceInstance.stats;
        }
    }
    private void Update()
    {
        if (Player == null) return;
        CursorCoordinates();
        Vector3 _ovjPos = Player.position + _direction * Radius;
        transform.position = _ovjPos;
        transform.right = _direction;


    }
    public void CursorCoordinates()
    {
        if (Mouse.current == null || Camera.main == null) return;
        // 마우스 위치를 월드기준으로 전환
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(
        new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z))
    );
        _mouseWorldPos.z = 0f;
        // 방향백터 계산
        _direction = (_mouseWorldPos - Player.position).normalized;
    }
    public void SwitchCoroutine(IEnumerator newRoutine)
    {
        // 코루틴 스위칭 함수
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _currentRoutine = StartCoroutine(newRoutine);
    }

    public IEnumerator Axe()
    {
        // 무한 반복
        while (true)
        {
            // TODO : 상점 전환이 씬전환/온오프 인지 확인후에 자동공격 멈춤 로직 추가
            if (_playerScript != null && _playerScript.isDead)// 플레이어가 사망하면 공격중단
            {
                Debug.Log("공격 중단");//코루틴 중단
                yield break;
            }

            if (_axePerfab != null)
            {
                GameObject _axe = Instantiate(_axePerfab, _muzzlePos.position, Quaternion.identity);
                _axe.transform.right = _direction;
                float atk = _playerStats.CurrentAtk; // 플레이어의 공격력값을 가져옴
                Attack attackScript = _axe.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk); // 공격력값을 생성된 오브젝트로 이동
                }
                Destroy(_axe, _meleeAttack);
            }

            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    public IEnumerator Sword()
    {
        // 무한 반복
        while (true)
        {
            // TODO : 상점 전환이 씬전환/온오프 인지 확인후에 자동공격 멈춤 로직 추가
            if (_playerScript != null && _playerScript.isDead)// 플레이어가 사망하면 공격중단
            {
                Debug.Log("공격 중단");
                yield break;
            }

            if (_swordPerfab != null)
            {
                GameObject _sword = Instantiate(_swordPerfab, _muzzlePos.position, Quaternion.identity);
                _sword.transform.right = _direction;
                float atk = _playerStats.CurrentAtk; // 플레이어의 공격력값을 가져옴
                Attack attackScript = _sword.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk); // 공격력값을 생성된 오브젝트로 이동
                }
                Destroy(_sword, _meleeAttack);
            }

            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    public IEnumerator Fire()
    {
        // 무한 반복
        while (true)
        {
            // TODO : 상점 전환이 씬전환/온오프 인지 확인후에 자동공격 멈춤 로직 추가
            if (_playerScript != null && _playerScript.isDead)// 플레이어가 사망하면 공격중단
            {
                Debug.Log("공격 중단");
                yield break;
            }

            if (_bulletPerfab != null)
            {
                GameObject _bullet = Instantiate(_bulletPerfab, _muzzlePos.position, Quaternion.identity);
                _bullet.transform.right = _direction;
                float atk = _playerStats.CurrentAtk; // 플레이어의 공격력값을 가져옴
                Attack attackScript = _bullet.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk); // 공격력값을 생성된 오브젝트로 이동
                }
                Destroy(_bullet, _bulletTime);
            }

            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    private float GetAttackDelay()
    {
        float atkSpeed = _playerStats.AtkSpd;
        if (atkSpeed <= 0.01f)
        {
            // 공격속도가 비정상이면 자동으로 5로 맞춤
            atkSpeed = 5f;
        }
        return 1f / atkSpeed;
    }

}
