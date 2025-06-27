using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 ID, 프리팹 ID, 데미지, 갯수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private float _timer;
    public PlayerController player;

    private void Awake()
    {
        player = GameManager.instance.playerController;
    }

    public void Init(ItemDataScriptableObject data)
    {
        // Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;     // localPosition을 원점으로

        //Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        // poolManager 안에 있는 prefab 배열의 길이까지
        for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
        {
            // 만약 투사체가 prefab index와 같다면
            if (data.projectile == GameManager.instance.poolManager.prefabs[i])
            {
                // poolManager에서 찾아 초기화 (prefabId = index)
                prefabId = i;
                break;
            }
        }

        switch (id)
        {   
            case 0:
                // 시계 방향으로 돌기 위해 - 사용
                speed = 150;
                WeaponPosition();
                break;

            default:
                // 일단 총알 생성 속도를 speed로 놔둠
                speed = 0.3f;
                break;
        }

        // 특정 함수 호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyEquipment", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                // 무기 회전
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

    private void WeaponPosition()
    {
        // 반복문을 통해 poolManager에 있는 프리팹을 가져오게
        for (int i = 0; i < count; i++)
        {
            Transform meleeWeapon;

            if (i < transform.childCount)
            {
                // 기존 오브젝트 우선 활용
                meleeWeapon = transform.GetChild(i);
            }

            else
            {
                // 모자랄 시 풀 안에서 가져와서 활용
                meleeWeapon = GameManager.instance.poolManager.ReturnObject(prefabId).transform;
                meleeWeapon.parent = transform;
            }

            // 무기 위치 초기화
            meleeWeapon.localPosition = Vector3.zero;

            // 무기 회전 초기화
            meleeWeapon.localRotation = Quaternion.identity;

            // 무기 회전 각도를 위한 코드
            Vector3 rotationVec = Vector3.forward * 360 * i / count;

            // 각도 적용
            meleeWeapon.Rotate(rotationVec);

            // 위쪽으로 이동, 플레이어와의 거리, 이동 방향을 world로 변경
            meleeWeapon.Translate(meleeWeapon.up * 2.5f, Space.World);

            // 관통력을 무한으로 만들기 위해 -1 입력
            meleeWeapon.GetComponent<MeleeWeapon>().Init(damage, -1, Vector3.zero);
        }
    }

    private void Fire()
    {
        // 플레이어 근처에 적이 없다면 반환
        if (!player.scanner.nearestTarget) return;
        
        Vector3 targetPos = player.scanner.nearestTarget.position;
    
        // 크기 포함 방향 : 목표 위치 - 자신 위치
        Vector3 dir = targetPos - transform.position;
        
        // 정규화
        dir = dir.normalized;
    
        Transform bullet = GameManager.instance.poolManager.ReturnObject(prefabId).transform;
        bullet.position = transform.position;
    
        // 지정한 축을 중심으로 목표를 향해 회전(z축)
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    
        bullet.GetComponent<MeleeWeapon>().Init(damage, count, dir);
    }
}
