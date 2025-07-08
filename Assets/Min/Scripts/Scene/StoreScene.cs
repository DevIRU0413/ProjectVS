using System.Collections;
using System.Collections.Generic;

using ProjectVS.CharacterImages.EventSpriteChangeManager;
using ProjectVS.Dialogue.ChoiceDialogueManager;
using ProjectVS.Dialogue.DialogueLogManager;
using ProjectVS.Dialogue.DialogueManagerR;
using ProjectVS.Dialogue.TextEffect.DialogueTextTyper;
using ProjectVS.Item.BuyItemObjBehaviour;
using ProjectVS.Item.ItemManager;
using ProjectVS.Manager;

using TMPro;

using UnityEngine;


namespace ProjectVS.Scene.StoreScene
{
    public class StoreScene : SceneBase
    {
        public override SceneID SceneID => SceneID.StoreScene;
        public GameObject SpawnPoint;

        [SerializeField] private DialogueTextTyper _repeatText;
        [SerializeField] private DialogueTextTyper _eventText;

        [SerializeField] private List<BuyItemObjBehaviour> _buyItems;

        [SerializeField] private DialogueLogManager _dialogueLogManager;
        [SerializeField] private ChoiceDialogueManager _choiceDialogueManager;
        [SerializeField] private EventSpriteChangeManager _eventSpriteChangeManager;

        protected override void Initialize()
        {
            SpawnPlayer();
            AssignText();
            AssignDialogueSubManagers();
        }

        private void Start()
        {
            AssignBuyObjects();
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

        private void AssignText()
        {
            if (_repeatText == null) Debug.LogWarning($"[StoreScene] RepeatText가 등록되지 않았습니다");
            if (_eventText == null) Debug.LogWarning($"[StoreScene] EventText가 등록되지 않았습니다");

            DialogueManagerR.Instance.AssignTextWhenSceneChanged(_eventText, _repeatText, _eventText, _eventText);
        }

        private void AssignBuyObjects()
        {
            ItemManager.Instance.AssignBuyItemObjects(_buyItems);
        }

        private void AssignDialogueSubManagers()
        {
            DialogueManagerR.Instance.AssignSubManagers(_dialogueLogManager, _choiceDialogueManager, _eventSpriteChangeManager);
        }
    }
}
