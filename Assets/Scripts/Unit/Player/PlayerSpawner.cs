using ProjectVS.Manager;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerSpawner : SimpleSingleton<PlayerSpawner>
    {
        [Header("Player Prefabs by Class")]
        public GameObject swordPrefab;
        public GameObject axePrefab;
        public GameObject magicPrefab;

        private PlayerDataManager _playerDataManager;

        // Out Use Field
        public GameObject CurrentPlayer { get; private set; }

        protected override void Awake()
        {
            // 데이터 캐싱

        }

        private void Start()
        {
            CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
            _playerDataManager = PlayerDataManager.Instance;
        }

        public GameObject SpawnPlayer(Vector3 position)
        {
            // 리스폰 시 기존 제거
            if (CurrentPlayer != null)
            {
                Destroy(CurrentPlayer);
            }

            // 프리팹 생성
            CharacterClass pClass = PlayerDataManager.Instance.Stats.CharacterClass;
            GameObject prefab = GetPrefab(pClass);
            if (prefab == null)
            {
                Debug.LogError($"[PlayerSpawner] 클래스에 해당하는 프리팹이 없습니다: {pClass}");
                return null;
            }

            // 세팅
            GameObject player = Instantiate(prefab, position, Quaternion.identity);
            JDW.PlayerConfig config = player.GetComponent<JDW.PlayerConfig>();
            PlayerStats stats = PlayerDataManager.Instance.Stats;

            // 플레이어 능력치
            if (stats == null)
            {
                Debug.LogError("플레이어 기본 능력치 데이터를 불어오지 못했습니다.");
                return null;
            }
            config.Stats = stats;

            // 플레이어 아이템 초기화
            CurrentPlayer = player;
            return player;
        }

        private GameObject GetPrefab(CharacterClass type)
        {
            return type switch
            {
                CharacterClass.Sword => swordPrefab,
                CharacterClass.Axe => axePrefab,
                CharacterClass.Magic => magicPrefab,
                _ => null
            };
        }
    }
}
