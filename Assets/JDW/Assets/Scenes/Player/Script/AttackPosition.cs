using System.Collections;

using ProjectVS;

using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEditor.Experimental.GraphView.GraphView;

namespace ProjectVS.JDW
{
    public class AttackPosition : MonoBehaviour
    {
        [SerializeField] private Transform _muzzlePos;
        [SerializeField] private GameObject _attackPrefab;
        [SerializeField] private float _attackDuration = 0f;
        [SerializeField] private float _attackOffset = 0f;

        private PlayerConfig _player;

        private void Start()
        {
            Debug.Log($"[AttackPosition] Start 호출됨 on {gameObject.name}");

            _player = GetComponentInParent<PlayerConfig>();
            if (_player == null || _attackPrefab == null)
            {
                Debug.LogWarning("AttackPosition 초기화 실패: _player 또는 _attackPrefab 없음");
                return;
            }

            Debug.Log("AttackRoutine 시작");
            StartCoroutine(AttackRoutine(_attackPrefab, _attackDuration, _attackOffset)); // 생성 프리팹 / 사라지는 속도 / 플레이어와의 거리
        }
        private IEnumerator AttackRoutine(GameObject prefab, float duration, float offset)
        {
            Debug.Log("코루틴 시작");
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

                    Vector3 direction = (mouseWorldPos - _player.transform.position);
                    if (direction.sqrMagnitude < 0.01f)
                    {
                        direction = Vector3.right;
                    }
                    direction.Normalize();

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Vector3 spawnPos = _player.transform.position + direction * 0.5f;

                    GameObject instance = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                    instance.GetComponent<Attack>()?.SetDamage(_player.Stats.CurrentAtk);

                    float elapsed = 0f;
                    while (elapsed < duration)
                    {
                        if (instance == null || _player == null)
                        {
                            break;
                        }

                        instance.transform.position = _player.transform.position + direction * offset;
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
}

