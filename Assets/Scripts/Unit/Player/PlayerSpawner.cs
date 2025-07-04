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

        public GameObject CurrentPlayer { get; private set; }

        public GameObject SpawnPlayer(Vector3 position, CharacterClass classType, PlayerStats stats)
        {
            if (CurrentPlayer != null)
            {
                Destroy(CurrentPlayer); // 리스폰 시 기존 제거
            }

            GameObject prefab = GetPrefab(classType);
            if (prefab == null)
            {
                Debug.LogError($"[PlayerSpawner] 클래스에 해당하는 프리팹이 없습니다: {classType}");
                return null;
            }

            GameObject player = Instantiate(prefab, position, Quaternion.identity);
            var config = player.GetComponent<PlayerConfig>();
            config.Init(stats);

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
