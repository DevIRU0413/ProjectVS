using ProjectVS.Manager;
using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Scene
{
    public class InGameScene : SceneBase
    {
        public override SceneID SceneID => SceneID.InGameScene;

        public GameObject SpawnPoint;

        protected override void Initialize()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            if (PlayerSpawner.ForceInstance.CurrentPlayer != null) return;

            Vector3 spawnPos = Vector3.zero;
            if (SpawnPoint == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
                if (go != null)
                    spawnPos = go.transform.position;
            }
            else
                spawnPos = SpawnPoint.transform.position;

            var stats = PlayerDataManager.ForceInstance.Stats;
            var classType = stats.CharacterClass;

            PlayerSpawner.ForceInstance.SpawnPlayer(spawnPos, classType, stats);
        }
    }
}
