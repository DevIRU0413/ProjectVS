using System.Collections;

using ProjectVS;

using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackPosition : MonoBehaviour
{
    public Transform Player;
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private GameObject _axePrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzlePos;

    private Coroutine _currentRoutine;
    private PlayerConfig _player;

    private void Awake()
    {
        if (Player != null)
        {
            _player = Player.GetComponent<PlayerConfig>();
        }
    }
    private void Update()
    {
        if (Player == null) return;
    }
    public void SwitchCoroutine(IEnumerator newRoutine)
    {
        // 코루틴 스위치 함수
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _currentRoutine = StartCoroutine(newRoutine);
    }

    public IEnumerator Axe()
    {
        return AttackRoutine(_axePrefab, 0.4f, 1f);
    }

    public IEnumerator Sword()
    {
        return AttackRoutine(_swordPrefab, 0.2f, 1f);
    }

    public IEnumerator Fire()
    {
        return AttackRoutine(_bulletPrefab, 0.7f, 0.5f);
    }

    private IEnumerator AttackRoutine(GameObject prefab, float duration, float offset)
    {
        while (true)
        {
            if (_player != null && _player.IsDead)
            {
                yield break;
            }

            if (prefab != null)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mouseWorldPos.z = 0f;

                Vector3 direction = (mouseWorldPos - Player.position);
                if (direction.sqrMagnitude < 0.01f)
                {
                    direction = Vector3.right;
                }
                direction.Normalize();

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Vector3 spawnPos = Player.position + direction * 0.5f;

                GameObject instance = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                instance.GetComponent<Attack>()?.SetDamage(_player.Stats.CurrentAtk);

                float elapsed = 0f;
                while (elapsed < duration)
                {
                    if (instance == null || Player == null)
                    {
                        break;
                    }

                    instance.transform.position = Player.position + direction * offset;
                    instance.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                Destroy(instance);
            }

            yield return new WaitForSeconds(GetAttackDelay());
        }
    }

    private float GetAttackDelay()
    {
        float atkSpeed = _player.Stats.AtkSpd;
        if (atkSpeed <= 0.01f)
        {
            atkSpeed = 5f; // 최소 공격속도 보정
        }
        return 1f / atkSpeed;
    }
}

