using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.Shop.NPCAffinityModel
{
    public class NPCAffinityModel : MonoBehaviour
    {
        private int _affinityExp = 0;
        private int _affinityLevel = 0;

        private const int AFFINITY_EXP_MAX = 100;
        private const int AFFINITY_EXP_MIN = 0;
        private const int AFFINITY_LEVEL_MAX = 100;
        private const int AFFINITY_LEVEL_MIN = 0;


        public int AffinityLevel => _affinityLevel;
        public int AffinityExp => _affinityExp;
        public int AffinityExpMax => AFFINITY_EXP_MAX;


        public void IncreaseAffinity(int amount)
        {
            _affinityExp += amount;

            while (_affinityExp >= AFFINITY_EXP_MAX)
            {
                _affinityLevel++;
                _affinityExp -= AFFINITY_EXP_MAX;

                _affinityLevel = Mathf.Clamp(_affinityLevel, AFFINITY_LEVEL_MIN, AFFINITY_LEVEL_MAX);
            }
        }
    }
}
