using UnityEngine;
using ProjectVS.ItemYSJ;
using ProjectVS.Unit;
using ProjectVS.Data.Player;

namespace ProjectVS.Unit.Player
{
    [RequireComponent(typeof(PlayerConfig))]
    public class ProjectileLauncher : MonoBehaviour
    {
        [Header("발사 설정")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject projectilePrefab;

        private float atkSpeed = 1.0f;
        private float timer = 0f;
        private float range = 10f;
        private float sizeMultiplier = 1f;
        private int extraProjectileCount = 0;

        private int baseEffectValue = 10; // ItemEffectValue (Item 기준 배율)
        private Transform currentTarget;

        private ItemDataSO currentWeaponData;
        private PlayerStats stats;

        private void Awake()
        {
            var config = GetComponent<PlayerConfig>();
            stats = config.Stats;
        }

        private void Update()
        {
            if (currentWeaponData == null || stats == null) return;

            timer += Time.deltaTime;
            if (timer >= atkSpeed)
            {
                Fire();
                timer = 0f;
            }
        }

        public void RegisterWeapon(ItemDataSO weaponData)
        {
            currentWeaponData = weaponData;

            atkSpeed = weaponData.ItemAtkSpeed;
            range = weaponData.Range;
            sizeMultiplier = weaponData.Size;
            baseEffectValue = weaponData.ItemEffectValue;

            extraProjectileCount = 0; // 세트 효과 초기화
        }

        public void SetTarget(Transform target)
        {
            currentTarget = target;
        }

        public void SetSizeMultiplier(float value)
        {
            sizeMultiplier = value;
        }

        public void AddExtraProjectile(int count)
        {
            extraProjectileCount += count;
        }

        private void Fire()
        {
            if (firePoint == null || projectilePrefab == null) return;

            Vector2 fireDirection = GetFireDirection();
            int totalShots = 1 + extraProjectileCount;

            for (int i = 0; i < totalShots; i++)
            {
                Vector2 shootDir = fireDirection;

                // 여러 발사체 방향 분산
                if (totalShots > 1)
                {
                    float angleOffset = (i - totalShots / 2f) * 10f;
                    shootDir = Quaternion.Euler(0, 0, angleOffset) * shootDir;
                }

                GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                proj.transform.localScale *= sizeMultiplier;

                var projectile = proj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    int finalDmg = Mathf.RoundToInt(stats.CurrentAtk * (baseEffectValue / 10f));
                    projectile.Fire(shootDir.normalized, currentTarget);
                }
            }
        }

        private Vector2 GetFireDirection()
        {
            // 1순위: 타겟 추적
            if (currentTarget != null)
                return (currentTarget.position - firePoint.position).normalized;

            // 2순위: 마우스 방향
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return (mouseWorld - firePoint.position).normalized;
        }
    }
}
