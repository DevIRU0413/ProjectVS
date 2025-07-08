using System.Collections.Generic;

using ProjectVS.Item.GetItemButtonBehaviour;
using ProjectVS.Item.ItemManager;
using ProjectVS.Manager;
using ProjectVS.UIs.CutSceneEffect.CutSceneController;
using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Scene
{
    public class InGameScene : SceneBase
    {
        public override SceneID SceneID => SceneID.InGameScene;

        public GameObject SpawnPoint;

        [SerializeField] private List<GetItemButtonBehaviour> _buttons;

        protected override void Initialize()
        {
            SpawnPlayer();
        }

        private void Start()
        {
            CheckCanShowCGScene();
            AssignItemManagerField();
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

            PlayerSpawner.ForceInstance.SpawnPlayer(spawnPos);
        }

        private void CheckCanShowCGScene()
        {
            if (PlayerDataManager.Instance.BattleSceneCount <= 1)
            {
                if (CutSceneController.Instance == null)
                {
                    Debug.LogWarning($"[InGameScene] CutSceneController.Instance를 찾을 수 없습니다");
                }
                else
                {
                    CutSceneController.Instance.PlayCutScene(CutSceneType.Opening);
                }
            }
        }

        private void AssignItemManagerField()
        {
            ItemManager.Instance.AssignGetItemButtons(_buttons);
        }
    }
}
