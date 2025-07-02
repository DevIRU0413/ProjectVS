using System.Collections;
using System.Collections.Generic;
using ProjectVS.Manager;

using UnityEngine;


namespace ProjectVS.Scene.MainMenuScene
{
    public class StoreScene : SceneBase
    {
        public override SceneID SceneID => SceneID.StoreScene;
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

            //var stats = PlayerDataManager.ForceInstance.stats;
            //var classType = stats.CharacterClass;

            //Unit.Player.PlayerSpawner.ForceInstance.SpawnPlayer(spawnPos, classType, stats);
        }
    }
}
