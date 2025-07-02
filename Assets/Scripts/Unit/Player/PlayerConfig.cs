using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerConfig : MonoBehaviour
    {
        public PlayerStats Stats { get; private set; }

        public void Init(PlayerStats stats)
        {
            Stats = stats;
            GetComponent<PlayerHealthSystem>()?.Init(Stats);
            GetComponent<PlayerLevelSystem>()?.Init(Stats);
        }

        public PlayerHealthSystem Health => _health ??= GetComponent<PlayerHealthSystem>();
        public PlayerLevelSystem Level => _level ??= GetComponent<PlayerLevelSystem>();
        public ProximityScanner Scanner => _scanner ??= GetComponent<ProximityScanner>();

        private PlayerHealthSystem _health;
        private PlayerLevelSystem _level;
        private ProximityScanner _scanner;
    }
}
