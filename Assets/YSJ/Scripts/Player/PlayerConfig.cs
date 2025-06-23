using UnityEngine;

namespace PVS.Player
{
    // 외부 값 변경용
    public class PlayerConfig : MonoBehaviour
    {
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float ATK { get; private set; }
        [field: SerializeField] public float SPD { get; private set; }

        [field: SerializeField] public float AtkRAnge { get; private set; }
    }
}
