using ProjectVS.Unit.Player;
using ProjectVS.Monster;
using Unity.VisualScripting;

using UnityEngine;
using ProjectVS.JDW;

namespace ProjectVS.Item
{
    public class ItemEffectHandler : MonoBehaviour
    {
        [SerializeField] ItemData _data;

        [SerializeField] LayerMask _targetLayer;

        [SerializeField]  GameObject _projectilePrefab;
        [SerializeField]  GameObject _throwPrfab;

        [SerializeField] PlayerStats _playerStats;

        private int[] FiveWayAngle = new int[5] { -144, -72, 0, 72, 144, };
        private int[] EightWayAngle = new int[8] { -135, -90, -45, 0, 45, 90, 135, 180 };

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
                    Bomb(value);
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

        public  Vector2 GetMouseDirection2D(Transform from)
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

        public void ThreeShot(Transform user, float damage)
        {
            Vector2 dir = GetMouseDirection2D(user);

            for (int i = 0; i < 3; i++)
            {
                //TODO : 3점사 시간차 추가
                GameObject intance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);
                intance.GetComponent<Test_Projectile>().Init(dir, damage, 5f);
            }

            Debug.Log("Test : THREE SHOT");
        }

        public void Tornado(int damage)
        {
            //TODO : RaycastAll  또는 LineCast로 지지기 구현
        }

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

        public void Bomb(int damage)
        {
        }

        public void Double(Transform user ,int damage)
        {
            GameObject intance = Instantiate(_projectilePrefab, user.position, Quaternion.identity);

            Vector2 dir = GetMouseDirection2D(user);

            // 데미지 2번 적용 Projectile 클래스쪽에서 적용할지 or 이쪽에서 구현할지
            Debug.Log("Test : SHOT");
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
