using ProjectVS.Monster;

using UnityEngine;

namespace ProjectVS.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "ProjectVS/StageData", order = 1)]
    public class StageDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        public float clearTime = 900.0f;
        public int difficultyLevel = 1;

        [Header("보스 정보")]
        public MonsterController bossPrefab;

        [Header("클리어 보상")]
        public StageRewardData reward;
    }
}
