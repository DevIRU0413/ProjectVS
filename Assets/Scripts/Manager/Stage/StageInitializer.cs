using ProjectVS.Data;
using ProjectVS.Monster;
using ProjectVS.Unit.Player;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager.Stage
{
    public static class StageInitializer
    {
        public static StageContext BuildContext(GameObject root, StageDataSO stageData)
        {
            var ctx = new StageContext();

            // 0. stageData 주입
            ctx.StageData = stageData;

            // 1. PlayerStats 가져오기
            var stats = PlayerDataManager.ForceInstance.Stats;
            var classType = stats.CharacterClass;

            // 2. 스폰 위치 찾기
            Vector3 spawnPos = Vector3.zero;
            var spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint");
            if (spawnPoint != null)
                spawnPos = spawnPoint.transform.position;
            else
                Debug.LogWarning("[StageInitializer] PlayerSpawnPoint 태그를 찾지 못했습니다. (0,0) 사용");

            // 3. 플레이어 스포너 통해 소환
            GameObject playerGO = PlayerSpawner.Instance.CurrentPlayer;
            if (playerGO == null)
                playerGO = PlayerSpawner.Instance.SpawnPlayer(spawnPos, classType, stats);

            playerGO = PlayerSpawner.Instance.CurrentPlayer;
            PlayerConfig playerConfig = playerGO.GetComponent<PlayerConfig>();
            ctx.Player = playerConfig;

            // 4. 몬스터 스폰 초기화
            var spawner = root.GetOrAddComponent<MonsterSpawnController>();
            spawner.Init(playerGO, ctx.StageData.MaxSpawnableCount, ctx.StageData.spawnerConfig);
            ctx.Spawner = spawner;

            // 5. Boss (스폰은 나중에, 조건 체크용으로 참조만 가능)
            ctx.BossController = null;

            Debug.Log("[StageInitializer] StageContext 생성 완료");
            return ctx;
        }
    }
}
