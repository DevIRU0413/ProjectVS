using UnityEngine;

namespace ProjectVS.Monster.Data
{
    // 외부 값 변경용
    public class MonsterConfig : MonoBehaviour
    {
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float ATK { get; private set; }
        [field: SerializeField] public float SPD { get; private set; }

        [field: SerializeField] public float AtkRange { get; private set; }
        [field: SerializeField] public int ExpPoint { get; private set; }
    }
}
