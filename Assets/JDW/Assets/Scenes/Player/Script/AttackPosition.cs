using System.Collections;

using ProjectVS;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using static UnityEditor.Experimental.GraphView.GraphView;

namespace ProjectVS.JDW
{
    public class AttackPosition : MonoBehaviour
    {
        [SerializeField] private string _actionableSceneName = "BattleScene";
        [SerializeField] private Transform _muzzlePos;
        [SerializeField] private GameObject _attackPrefab;
        [SerializeField] private float _attackDuration = 0f;// 오브젝트의 지속시간
        [SerializeField] private float _attackOffset = 0f; // 플레이어와 오브젝트의 거리

        private PlayerConfig _player;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != _actionableSceneName) return;

            _player = GetComponentInParent<PlayerConfig>();
            if (_player == null || _attackPrefab == null)
            {
                Debug.LogWarning("AttackPosition 초기화 실패: _player 또는 _attackPrefab 없음");
                return;
            }

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

                    // 방향 구하기
                    Vector3 direction = (mouseWorldPos - _player.transform.position);
                    if (direction.sqrMagnitude < 0.01f)
                    {
                        direction = Vector3.right;
                    }
                    direction.Normalize();

                    // 해당 방향에 임의의 위치 지정 소환
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Vector3 spawnPos = _player.transform.position + direction * 0.5f;

                    // 해당 방향으로 돌려주기
                    GameObject instance = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
                    instance.GetComponent<Attack>()?.SetDamage(_player.Stats.CurrentAtk);

                    // 공격이 진행 되는 동안 플레이어와 몬스터의 거리 계산과 그에 맞는 회전을 해서 몬스터쪽으로 공격하게 지속 전환
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

                    // 그 지속 전환이 끝났을 때, 삭제
                    Destroy(instance);
                }
                yield return new WaitForSeconds(GetAttackDelay());
            }
        }

        private float GetAttackDelay()
        {
            float atkSpeed = _player.Stats.CurrentAtkSpd;
            if (atkSpeed <= 0.01f)
            {
                atkSpeed = 5f; // 최소 공격속도 보정
            }
            return 1f / atkSpeed;
        }
    }
}

