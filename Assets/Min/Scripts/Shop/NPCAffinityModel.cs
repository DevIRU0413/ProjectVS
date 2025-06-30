using System.Collections;
using System.Collections.Generic;

using ProjectVS.Util;

using UnityEngine;


namespace ProjectVS.Shop.NPCAffinityModel
{
    public class NPCAffinityModel : SimpleSingleton<NPCAffinityModel>
    {
        private int _affinityCurrentExp = 0;
        private int _affinityLevel = 1;

        private const int AFFINITY_EXP_MAX = 100;
        private const int AFFINITY_EXP_MIN = 0;
        private const int AFFINITY_LEVEL_MAX = 100;
        private const int AFFINITY_LEVEL_MIN = 1;


        public int AffinityLevel => _affinityLevel;

        public string AffinityLevelString
        {
            get
            {
                if (_affinityLevel >= AFFINITY_LEVEL_MAX)
                {
                    return "Max";
                }
                return _affinityLevel.ToString();
            }
        }
        public int AffinityCurrentExp => _affinityCurrentExp;
        public int AffinityExpMax => AFFINITY_EXP_MAX;


        public void IncreaseAffinity(int amount)
        {
            _affinityCurrentExp += amount;

            while (_affinityCurrentExp >= AFFINITY_EXP_MAX)
            {
                _affinityLevel++;
                _affinityCurrentExp -= AFFINITY_EXP_MAX;

                _affinityLevel = Mathf.Clamp(_affinityLevel, AFFINITY_LEVEL_MIN, AFFINITY_LEVEL_MAX);
            }
        }

        [ContextMenu("Test Up Affinity")]
        public void TestUpAffinity()
        {
            _affinityCurrentExp += 50;

            while (_affinityCurrentExp >= AFFINITY_EXP_MAX)
            {
                _affinityLevel++;
                _affinityCurrentExp -= AFFINITY_EXP_MAX;

                _affinityLevel = Mathf.Clamp(_affinityLevel, AFFINITY_LEVEL_MIN, AFFINITY_LEVEL_MAX);
            }

            Debug.Log($"[NPCAffinityModel] 현재 경험치: {_affinityCurrentExp}, 현재 레벨: {_affinityLevel}");
        }


        [ContextMenu("Test Full Up Affinity")]
        public void TestFullUpAffinity()
        {
            _affinityLevel = 20;

            Debug.Log($"[NPCAffinityModel] 현재 경험치: {_affinityCurrentExp}, 현재 레벨: {_affinityLevel}");
        }
    }
}
