using System.Collections;

using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Item
{
    public class ItemEffectHandler : MonoBehaviour
    {
        [SerializeField] ItemData _data;

        [SerializeField] LayerMask _targetLayer;

        [SerializeField] GameObject _projectilePrefab;
        [SerializeField] GameObject _throwPrfab;
        [SerializeField] GameObject _bombPrefab;

        [SerializeField] PlayerStats _playerStats;

        private int[] FiveWayAngle = new int[5] { -144, -72, 0, 72, 144, };
        private int[] EightWayAngle = new int[8] { -135, -90, -45, 0, 45, 90, 135, 180 };

        //============================================================================================
        Coroutine _effectRoutine;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ApplyEffect(_playerStats, _data);
            }
        }

        // 단순 힐 같이 단발성 아이템도 존재하기에 따로 뺴서 관리
        // 인벤에 귀속 기능 x
        public void ApplyEffect(PlayerStats stats, ItemData item)
        {
            Debug.Log("ApplyEffect - 1");

            /*  if (item == null || stats == null) return;*/
            int value = item.ItemEffectValue;

            switch (item.ItemEffect)
            {
                // ItemEffect Enum 값 : 0
                case ItemEffect.None:
                    Debug.Log($"[{item.ItemName}] 효과 없음");
                    break;

                // 1
                case ItemEffect.Restore:
                    stats.CurrentHp += value;
                    break;

                // 2
                case ItemEffect.Swing:
                    Swing(transform, value);
                    break;

                // 3 
                case ItemEffect.Shot:
                    Shot(transform, value);
                    break;

                // 4
                case ItemEffect.RandomShot:
                    RandomShot(transform, value);
                    break;

                // 5
                case ItemEffect.Spin:
                    Spin(transform, item.ItemRange, value);
                    break;

                // 6
                case ItemEffect.HPStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.MaxHp, value);
                    break;

                // 7
                case ItemEffect.DefStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.Dfs, value);
                    break;

                // 8
                case ItemEffect.SpeedStatUP:
                    stats.SetIncreaseBaseStats(UnitStaus.Spd, value);
                    break;

                // 9
                case ItemEffect.ThreeShot:
                    ThreeShot(transform, value);
                    break;

                // 10
                case ItemEffect.Tornado:
                    Tornado(value);
                    break;

                // 11
                case ItemEffect.Throw:
                    Throw(transform, value);
                    break;

                // 12
                case ItemEffect.Fiveway:
                    Fiveway(transform, value);
                    break;

                // 13
                case ItemEffect.Bomb:
                    Bomb(transform, value);
                    break;

                // 14
                case ItemEffect.Double:
                    Double(transform, value);
                    break;

                // 15
                case ItemEffect.Eightway:
                    Eightway(transform, value);
                    break;

                default:
                    Debug.Log($"[ItemEffect] 처리되지 않은 효과: {item.ItemEffect}");
                    break;
            }
        }

        /// <summary>
        /// 마우스 입력 지점 위치 반환 함수
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public Vector2 GetMouseDirection2D(Transform from)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0f;

            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            worldMousePos.z = 0f;

            Vector2 direction = ((Vector2)worldMousePos - (Vector2)from.position).normalized;
            return direction;
        }

        public void Swing(Transform transform, float damage, float radius = 1.5f, float angle = 90f)
        {
            Vector2 origin = transform.position;
            Vector2 swingDir = GetMouseDirection2D(transform);

            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, _targetLayer);

            foreach (Collider2D hit in hits)
            {
                Vector2 target = (Vector2)hit.transform.position - origin;

                if (target.magnitude > radius)
                    continue;

                float attackAngle = Vector2.Angle(swingDir, target);

                if (attackAngle <= angle * 0.5f)
                {
                    hit.GetComponent<Test_Monster>()?.TakeDamage(damage);
                    Debug.Log($"[Swing] Hit: {hit.name}");
                }
            }
        }

        public void Shot(Transform user, float damage)
        {
            GameObject intance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);

            Vector2 dir = GetMouseDirection2D(user);
            intance.GetComponent<Test_Projectile>().Init(dir, damage, 5f);

            Debug.Log("Test : SHOT");
        }

        public void RandomShot(Transform user, float damage)
        {
            // insideUnitCircle : 2D상에서 transform.position 기준 반지름 1인 원의 랜덤 방향의 벡터 반환, 
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

            GameObject intance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);
            intance.GetComponent<Test_Projectile>().Init(randomDir, damage, 5f);

            Debug.Log("Test : RANDOM SHOT");
        }

        public void Spin(Transform user, float radius, float damage)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(user.position, radius, _targetLayer);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<Test_Monster>()?.TakeDamage(damage);
            }
            Debug.Log("Test : SPIN");
        }

        #region ThreeShot(Transform user, float damage)
        public void ThreeShot(Transform user, float damage)
        {
            StartCoroutine(ThreeShotCoroutine(user, damage));
        }

        private IEnumerator ThreeShotCoroutine(Transform user, float damage)
        {
            Vector2 dir = GetMouseDirection2D(user);
            WaitForSeconds ThreeShotDelay = new(0.2f);

            for (int i = 0; i < 3; i++)
            {
                GameObject instance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);
                instance.GetComponent<Test_Projectile>().Init(dir, damage, 5f);
                yield return ThreeShotDelay;
            }
        }
        #endregion

        #region Tornado(int damage)
        public void Tornado(int damage)
        {
            float duration = 3f; // 지속 시간

            float range = 3f; // 범위 세로s
            float width = 1.5f; // 범위 가로

            Vector2 dir = (_spriteRenderer.flipX ? -transform.right : transform.right) + new Vector3(0.4f, 0f, 0f);

            if (_effectRoutine == null)
            {
                _effectRoutine = StartCoroutine(TornadoCoroutine(transform, damage, duration, range, width, dir));
            }

            float Timer = duration; Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                StopCoroutine(_effectRoutine);
            }
        }

        WaitForSeconds TornadoDelay = new(0.3f);
        private IEnumerator TornadoCoroutine(Transform user, int damage, float duration, float range, float width, Vector2 dir)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                Collider2D[] hits = Physics2D.OverlapBoxAll(user.position, new Vector2(range, width), 0);
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Monster"))
                    {
                        hit.GetComponent<Test_Monster>()?.TakeDamage(damage);
                        Debug.Log($"Tornado 대상 : {hit.name} / 데미지 : {damage}");
                    }
                }

                yield return TornadoDelay;
                elapsed += 0.3f;
            }

            _effectRoutine = null;
            Debug.Log("Tornado 종료");
        }

        private void OnDrawGizmos()
        {
            Vector2 dir = transform.position + new Vector3(0.2f, 0f, 0f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + (Vector3)dir, new Vector3(3f, 0.5f, 0));
        }
        #endregion

        public void Throw(Transform user, int damage, float range = 3f, float radius = 1.5f)
        {
            Vector2 randomOffset = Random.insideUnitCircle * range;
            Vector2 spawnPosition = (Vector2)user.position + randomOffset;

            GameObject instance = Instantiate(_throwPrfab, spawnPosition, Quaternion.identity);
            //TODO : 몇 초후 떨어지는 등, 생성 지점 표시 등 구현 
        }

        public void Fiveway(Transform user, int damage, float spacingAngle = 72f)
        {
            Vector2 dir = user.transform.right; // 기준 방향 (예: 오른쪽)

            foreach (float angle in FiveWayAngle)
            {
                Vector2 shootDir = Quaternion.Euler(0, 0, angle) * dir;
                GameObject instance = Instantiate(_projectilePrefab, user.transform.position, Quaternion.identity);

                Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                rb.velocity = shootDir.normalized * 5f;
            }

            Debug.Log("Test : FIVE WAY");
        }

        public void Bomb(Transform user, float damage)
        {
            GameObject intance = Instantiate(_bombPrefab, user.position, Quaternion.identity);

            Vector2 dir = GetMouseDirection2D(user);
            Test_Projectile projectile = intance.GetComponent<Test_Projectile>();
            projectile.Init(dir, damage, 5f);
            projectile.Bomb(damage);
        }

        public void Double(Transform user, int damage)
        {
            GameObject intance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);

            Vector2 dir = GetMouseDirection2D(user);

            Debug.Log("Test : Double");
        }

        public void Eightway(Transform user, int damage, float spacingAngle = 72f)
        {
            Vector2 dir = user.transform.right; // 기준 방향 (예: 오른쪽)

            foreach (float angle in EightWayAngle)
            {
                Vector2 shootDir = Quaternion.Euler(0, 0, angle) * dir;
                GameObject instance = Instantiate(_projectilePrefab, user.transform.position, Quaternion.identity);

                Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                rb.velocity = shootDir.normalized * 5f;
            }

            Debug.Log("Test : EIGHT WAY");
        }
    }
}
