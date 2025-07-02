using ProjectVS;
using ProjectVS.Manager;

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

            Unit.Player.PlayerSpawner.ForceInstance.SpawnPlayer(spawnPos, classType, stats);
        }
    }
}
