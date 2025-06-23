using UnityEngine;

namespace PVS.Monster
{
    // 외부 값 변경용
    public class MonsterConfig : MonoBehaviour
    {
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float ATK { get; private set; }
        [field: SerializeField] public float SPD { get; private set; }

        [field: SerializeField] public float AtkRange { get; private set; }
        [field: SerializeField] public int ExpPoint { get; private set; }

        // 드롭 아이템 넣을 수 있게 만들기
        // 확률 필수
        // 예) 골드 / 0.3f
    }
}
