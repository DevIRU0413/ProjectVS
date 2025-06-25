using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.Shop.NPCAffinityModel
{
    public class NPCAffinityModel : MonoBehaviour
    {
        [SerializeField] private int _affinity = 0;

        public int Affinity => _affinity;

        private const int AFFINITY_MAX = 100;
        private const int AFFINITY_MIN = 0;

        public void IncreaseAffinity(int amount)
        {
            _affinity += amount;
            _affinity = Mathf.Clamp(Affinity, AFFINITY_MIN, AFFINITY_MAX);
        }

        public void DecreaseAffinity(int amount)
        {
            _affinity -= amount;
            _affinity = Mathf.Clamp(Affinity, AFFINITY_MIN, AFFINITY_MAX);
        }
    }
}
