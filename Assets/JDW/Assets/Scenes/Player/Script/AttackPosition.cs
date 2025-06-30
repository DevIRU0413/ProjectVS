using System.Collections;

using ProjectVS;

using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPosition : MonoBehaviour
{
    public Transform Player;
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private GameObject _axePrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzlePos;

    private Coroutine _currentRoutine;
    private PlayerConfig _playerScript;

    private void Awake()
    {
        if (Player != null)
        {
            _playerScript = Player.GetComponent<PlayerConfig>();
        }
    }
    private void Update()
    {
        if (Player == null) return;
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

    private IEnumerator FollowAndDestroy(GameObject obj, Transform target, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            if (obj == null || target == null)
                yield break;

            obj.transform.position = target.position;

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }
    public IEnumerator Axe()
    {
        while (true)
        {
            if (_playerScript != null && _playerScript.isDead)
                yield break;

            if (_axePrefab != null)
            {
                // 마우스 방향 계산
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));
                mouseWorldPos.z = 0f;
                Vector3 direction = (mouseWorldPos - Player.position).normalized;

                
                Vector3 spawnPos = Player.position + direction * 0.5f;

                GameObject axe = Instantiate(_axePrefab, spawnPos, Quaternion.identity);
                axe.transform.right = direction;

                // 공격력 설정
                float atk = _playerScript.Stats.CurrentAtk;
                Attack attackScript = axe.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk);
                }

                // 지정한 시간동안 플레이어를 따라감
                float duration = 0.4f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    if (axe == null || Player == null)
                        break;

                    axe.transform.position = Player.position + direction * 1f; // 플레이어보다 앞쪽으로 지정한 단위 거리 위치에 생성
                    axe.transform.right = direction;

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                Destroy(axe);
            }

            // 공격속도가 높아질수록 딜레이가 짧아짐
            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    public IEnumerator Sword()
    {
        // 무한 반복
        while (true)
        {
            if (_playerScript != null && _playerScript.isDead)
                yield break;

            if (_swordPrefab != null)
            {
                // 마우스 방향 계산
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));
                mouseWorldPos.z = 0f;
                Vector3 direction = (mouseWorldPos - Player.position).normalized;


                Vector3 spawnPos = Player.position + direction * 0.5f;

                GameObject sword = Instantiate(_swordPrefab, spawnPos, Quaternion.identity);
                sword.transform.right = direction;

                // 공격력 설정
                float atk = _playerScript.Stats.CurrentAtk;
                Attack attackScript = sword.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk);
                }

                // 지정한 시간동안 플레이어를 따라감
                float duration = 0.2f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    if (sword == null || Player == null)
                        break;

                    sword.transform.position = Player.position + direction * 0.5f; // 플레이어보다 앞쪽으로 지정한 단위 거리 위치에 생성
                    sword.transform.right = direction;

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                Destroy(sword);
            }

            // 공격속도가 높아질수록 딜레이가 짧아짐
            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    public IEnumerator Fire()
    {
        // 무한 반복
        while (true)
        {
            if (_playerScript != null && _playerScript.isDead)
                yield break;

            if (_bulletPrefab != null)
            {
                // 마우스 방향 계산
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));
                mouseWorldPos.z = 0f;
                Vector3 direction = (mouseWorldPos - Player.position).normalized;


                Vector3 spawnPos = Player.position + direction * 0.5f;

                GameObject bulle = Instantiate(_bulletPrefab, spawnPos, Quaternion.identity);
                bulle.transform.right = direction;

                // 공격력 설정
                float atk = _playerScript.Stats.CurrentAtk;
                Attack attackScript = bulle.GetComponent<Attack>();
                if (attackScript != null)
                {
                    attackScript.SetDamage(atk);
                }

                // 지정한 시간동안 플레이어를 따라감
                float duration = 0.7f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    if (bulle == null || Player == null)
                        break;

                    bulle.transform.position = Player.position + direction * 0.5f; // 플레이어보다 앞쪽으로 지정한 단위 거리 위치에 생성
                    bulle.transform.right = direction;

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                Destroy(bulle);
            }

            // 공격속도가 높아질수록 딜레이가 짧아짐
            yield return new WaitForSeconds(GetAttackDelay());
        }
    }
    private float GetAttackDelay()
    {
        float atkSpeed = _playerScript.Stats.AtkSpd;
        if (atkSpeed <= 0.01f)
        {
            // 공격속도가 비정상이면 자동으로 5로 맞춤
            atkSpeed = 5f;
        }
        return 1f / atkSpeed;
    }

}
