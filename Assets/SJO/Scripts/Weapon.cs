using ProjectVS.JDW;

using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public string poolKey;
    public float damage;
    public int count;
    public float speed;

    public float timer;
    public PlayerController player;

    private ProjectVS.Util.PoolManager _poolManager;

    private void Awake()
    {
       // var config = GameManager.Instance.Player;
       // player = config.GetComponent<PlayerController>();
        _poolManager = ProjectVS.Util.PoolManager.ForceInstance;
    }

    public void Init(ItemDataScriptableObject data)
    {
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        // NEW Pool Key 설정 
        poolKey = data.projectile.name;

        // NEW Pool 등록 (안 되어 있으면)
        if (!_poolManager.HasPool(poolKey))
        {
            _poolManager.CreatePool(poolKey, data.projectile, 10);
        }

        switch (id)
        {
            case 0:
                speed = 150;
                WeaponPosition();
                break;

            case 1:
                // 일단 총알 연사 속도
                speed = 0.3f;
                break;
        }

        player.BroadcastMessage("ApplyEquipment", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            WeaponPosition();
        }

        player.BroadcastMessage("ApplyEquipment", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;

            case 1:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    private void WeaponPosition()
    {
        for (int i = 0; i < count; i++)
        {
            Transform meleeWeapon;

            if (i < transform.childCount)
            {
                meleeWeapon = transform.GetChild(i);
            }
            else
            {
                GameObject obj = _poolManager.Spawn(poolKey, Vector3.zero, Quaternion.identity);
                meleeWeapon = obj.transform;
                meleeWeapon.parent = transform;
            }

            meleeWeapon.localPosition = Vector3.zero;
            meleeWeapon.localRotation = Quaternion.identity;

            Vector3 rotationVec = Vector3.forward * 360 * i / count;
            meleeWeapon.Rotate(rotationVec);
            meleeWeapon.Translate(meleeWeapon.up * 2.5f, Space.World);

            meleeWeapon.GetComponent<MeleeWeapon>().Init(damage, -1, Vector3.zero);
        }
    }

    private void Fire()
    {
        // 플레이어 근처에 적이 없다면 반환
        Debug.Log(player.scanner.nearestTarget);
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;

        // 크기 포함 방향 : 목표 위치 - 자신 위치
        Vector3 dir = targetPos - transform.position;

        // 정규화
        dir = dir.normalized;

        Transform bullet = _poolManager.Spawn(poolKey, transform.position, Quaternion.identity).transform;

        // 지정한 축을 중심으로 목표를 향해 회전(z축)
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<RangeWeapon>().Init(damage, count, dir);
    }
}
