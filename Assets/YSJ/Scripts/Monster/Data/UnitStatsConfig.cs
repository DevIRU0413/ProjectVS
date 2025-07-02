using UnityEngine;

namespace ProjectVS.Monster.Data
{
    public class UnitStatsConfig : MonoBehaviour
    {
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float ATK { get; private set; }
        [field: SerializeField] public float DFS { get; private set; }
        [field: SerializeField] public float SPD { get; private set; }
        [field: SerializeField] public float ATKSPD { get; private set; }
    }
}
