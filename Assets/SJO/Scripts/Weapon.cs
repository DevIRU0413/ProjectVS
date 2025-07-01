using ProjectVS;
using static ProjectVS.Util.PoolManager;

using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public string poolKey;
    public float damage;
    public int count;
    public float speed;

    private float _timer;

    public PlayerConfig player;
    private ProjectVS.Util.PoolManager _poolManager;

    private void Awake()
    {
        player = GameManager.Instance.Player;
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

            default:
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

            default:
                _timer += Time.deltaTime;
                if (_timer > speed)
                {
                    _timer = 0f;
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
        if (!player.Scanner.nearestTarget) return;

        Vector3 targetPos = player.Scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;

        GameObject obj = _poolManager.Spawn(poolKey, transform.position, Quaternion.identity);
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        obj.GetComponent<MeleeWeapon>().Init(damage, count, dir);
    }
}
