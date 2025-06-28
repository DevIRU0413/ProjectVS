using System.Collections.Generic;

using ProjectVS.Interface;

using UnityEngine;

namespace ProjectVS.Monster
{
    public class MonsterSpawnController : MonoBehaviour
    {
        /// <summary>
        /// 몬스터 스폰 제어를 담당하는 컨트롤러.
        /// 다양한 유형의 스포너(IUnitSpawner 인터페이스 기반)를 지원하며,
        /// 그룹 별로 설정된 조건에 따라 주기적으로 몬스터를 생성
        /// </summary>
        [System.Serializable]
        public class SpawnEntry
        {
            public string groupName; // 그룹 식별용 이름 (디버깅용)
            public MonoBehaviour spawnerBehaviour; // 실제 IUnitSpawner를 구현한 스포너 컴포넌트
            public int unitCount = 10; // 한 번 생성 시 생성할 개체 수
            public float interval = 10f; // 스폰 주기 (초)
            public bool useTarget; // 타겟 기준 거리 검사 및 방향 지정 여부
            public GameObject target; // 타겟 객체 (예: 플레이어, 목표물 지정)
            public bool autoStart = true; // Start 시 자동 생성 여부
            public int maxSpawnedUnits = 100; // 누적 최대 생성 수 (초과 시 생성 중단)
            public float stopAfterTime = -1f; // 생성 중지 시간 제한 (-1이면 무제한)

            // 내부 상태 변수
            [HideInInspector] public float timer; // 스폰 타이머 누적값
            [HideInInspector] public int spawnedCount; // 누적 생성 개수
            [HideInInspector] public float activeTime; // 스폰 활성 시간

            // 스포너 인터페이스 접근용
            public IUnitSpawner Spawner => spawnerBehaviour as IUnitSpawner;
        }

        [Header("Spawn Groups")]
        public List<SpawnEntry> spawnEntries = new(); // 전체 스폰 그룹 목록

        private void Start()
        {
            // 초기화 및 autoStart 그룹 즉시 스폰
            foreach (var entry in spawnEntries)
            {
                entry.timer = 0f;
                entry.spawnedCount = 0;
                entry.activeTime = 0f;
                entry.target = GameManager.ForceInstance.Player.gameObject;

                if (entry.autoStart && CanSpawn(entry))
                {
                    Spawn(entry);
                }
            }
        }

        private void Update()
        {
            foreach (var entry in spawnEntries)
            {
                entry.timer += Time.deltaTime;
                entry.activeTime += Time.deltaTime;

                // 스폰 제한 시간 초과 시 skip
                if (entry.stopAfterTime > 0 && entry.activeTime >= entry.stopAfterTime)
                    continue;

                // 주기적 스폰 타이밍 도달 && 조건 만족 시 스폰
                if (entry.timer >= entry.interval && CanSpawn(entry))
                {
                    Spawn(entry);
                    entry.timer = 0f;
                }
            }
        }

        /// <summary>
        /// 실제 스폰 실행 (스포너 객체를 통해 유닛 생성 및 초기화)
        /// </summary>
        private void Spawn(SpawnEntry entry)
        {
            if (entry.Spawner == null)
            {
                Debug.LogWarning($"[SpawnController] Spawner is not valid in group: {entry.groupName}");
                return;
            }

            // 유닛 생성
            entry.Spawner.SpawnUnits(
                GameManager.Instance.Player.transform.position,
                entry.unitCount);

            // 유닛 초기화 (예: 타겟 방향 설정)
            entry.Spawner.InitUnits(entry.useTarget ? entry.target : null);

            // 누적 카운트 업데이트
            entry.spawnedCount += entry.unitCount;
        }

        /// <summary>
        /// 스폰 가능 여부 체크: 거리, 누적 수, 스포너 존재 유무 검사
        /// </summary>
        private bool CanSpawn(SpawnEntry entry)
        {
            if (entry.Spawner == null) return false;

            // 최대 생성 수 초과 시 스폰 금지
            if (entry.spawnedCount >= entry.maxSpawnedUnits)
                return false;

            return true;
        }
    }
}
