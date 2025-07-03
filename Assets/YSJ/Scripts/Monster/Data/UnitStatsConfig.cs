using ProjectVS.Manager;

using UnityEngine;

namespace ProjectVS.Monster.Data
{
    public class UnitStatsConfig : MonoBehaviour
    {
        [field: Header("ID")]
        [field: SerializeField] public int ID { get; private set; }

        [field: Header("Stats")]
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float ATK { get; private set; }
        [field: SerializeField] public float DFS { get; private set; }
        [field: SerializeField] public float SPD { get; private set; }
        [field: SerializeField] public float ATKSPD { get; private set; }

        protected virtual void Awake()
        {
            // if (GameManager.Instance.GamePlayType == GamePlayType.Test) return;
            // if (ID == 0) return;
        }
    }
}
