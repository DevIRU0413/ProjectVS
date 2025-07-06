using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    /// <summary>
    /// 몬스터 스폰 제어를 담당하는 컨트롤러.
    /// 다양한 유형의 스포너(IUnitSpawner 인터페이스 기반)를 지원하며,
    /// 그룹 별로 설정된 조건에 따라 주기적으로 몬스터를 생성
    /// </summary>
    [System.Serializable]
    public class SpawnEntry
    {
        public string GroupName;                                        // 그룹 식별용 이름 (디버깅용)
        public SpawnGroupType SpawnGroupType = SpawnGroupType.None;     // SpawnGroupType

        public List<GameObject> SpawnableObjects;                       // SpawnableObjects 

        public int GroupUnitSpawnCount = 10;                            // 한 번 생성 시 생성할 개체 수
        public float Interval = 10f;                                    // 스폰 주기 (초)

        public bool AutoStart = true;                                   // Start 시 자동 생성 여부
        public float StopAfterTime = -1f;                               // 생성 중지 시간 제한 (-1이면 무제한)

        // 내부 상태 변수
        [HideInInspector] public float timer;                           // 스폰 타이머 누적값
        [HideInInspector] public float activeTime;                      // 스폰 활성 시간
    }
}
