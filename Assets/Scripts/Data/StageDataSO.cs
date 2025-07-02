using UnityEngine;

namespace ProjectVS.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "ProjectVS/StageData", order = 1)]
    public class StageDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        [Min(0.01f)] public float ClearTime = 900.0f;
        [Min(0.01f)] public float DifficultyLevel = 1;
        [Range(1,1000)] public int MaxSpawnableCount = 1;

        [Header("몬스터 관련")]
        public MonsterSpawnControllerConfigSO spawnerConfig;

        [Header("보스 정보")]
        public GameObject BossPrefab;

        [Header("클리어 보상")]
        public StageRewardData Reward;
    }
}
