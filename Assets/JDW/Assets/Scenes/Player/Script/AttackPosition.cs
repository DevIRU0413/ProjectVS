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
            if (_playerScript != null && _playerScript.isDead) // 플레이 사망 시 멈춤
                yield break;

            if (_axePrefab != null)
            {
                // 커서 위치를 월드 좌표로 변환
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mouseWorldPos.z = 0f;
                // 방향 계산 마우스>플레이어
                Vector3 direction = (mouseWorldPos - Player.position);
                if (direction.sqrMagnitude < 0.01f) direction = Vector3.right;
                direction.Normalize();
                // 방향에 따라 z 축으로 회전
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // 플레이어와 공격 오브젝트의 거리
                Vector3 spawnPos = Player.position + direction * 0.5f;
                // 오브젝트 생성 / 회전 적용
                GameObject axe = Instantiate(_axePrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                // 공격력 정보를 가져와 적용
                axe.GetComponent<Attack>()?.SetDamage(_playerScript.Stats.CurrentAtk);


                // 일정 시간 동안 플레이어를 따라감
                float elapsed = 0f;
                float duration = 0.4f;
                while (elapsed < duration)
                {
                    if (axe == null || Player == null) break;

                    axe.transform.position = Player.position + direction * 1f;
                    axe.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
                // 오브젝트 제거
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
            if (_playerScript != null && _playerScript.isDead) // 플레이 사망 시 멈춤
                yield break;

            if (_axePrefab != null)
            {
                // 커서 위치를 월드 좌표로 변환
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mouseWorldPos.z = 0f;
                // 방향 계산 마우스>플레이어
                Vector3 direction = (mouseWorldPos - Player.position);
                if (direction.sqrMagnitude < 0.01f) direction = Vector3.right;
                direction.Normalize();
                // 방향에 따라 z 축으로 회전
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // 플레이어와 공격 오브젝트의 거리
                Vector3 spawnPos = Player.position + direction * 0.5f;
                // 오브젝트 생성 / 회전 적용
                GameObject sword = Instantiate(_swordPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                // 공격력 정보를 가져와 적용
                sword.GetComponent<Attack>()?.SetDamage(_playerScript.Stats.CurrentAtk);


                // 일정 시간 동안 플레이어를 따라감
                float elapsed = 0f;
                float duration = 0.2f;
                while (elapsed < duration)
                {
                    if (sword == null || Player == null) break;
                    sword.transform.position = Player.position + direction * 1f;
                    sword.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                // 오브젝트 제거
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
            if (_playerScript != null && _playerScript.isDead) // 플레이 사망 시 멈춤
                yield break;

            if (_axePrefab != null)
            {
                // 커서 위치를 월드 좌표로 변환
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mouseWorldPos.z = 0f;
                // 방향 계산 마우스>플레이어
                Vector3 direction = (mouseWorldPos - Player.position);
                if (direction.sqrMagnitude < 0.01f) direction = Vector3.right;
                direction.Normalize();
                // 방향에 따라 z 축으로 회전
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // 플레이어와 공격 오브젝트의 거리
                Vector3 spawnPos = Player.position + direction * 0.5f;
                // 오브젝트 생성 / 회전 적용
                GameObject bullet = Instantiate(_bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                // 공격력 정보를 가져와 적용
                bullet.GetComponent<Attack>()?.SetDamage(_playerScript.Stats.CurrentAtk);


                // 일정 시간 동안 플레이어를 따라감
                float elapsed = 0f;
                float duration = 0.7f; // 공격 유지시간
                while (elapsed < duration)
                {
                    if (bullet == null || Player == null) break;
                    bullet.transform.position = Player.position + direction * 0.5f; // 오브젝트가 생성 될 거리
                    bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                // 오브젝트 제거
                Destroy(bullet);
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
