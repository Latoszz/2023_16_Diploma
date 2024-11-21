using System.Collections.Generic;
using Audio;
using Events;
using InputScripts;
using Interaction.Objects;
using Interfaces;
using NPCScripts;
using SaveSystem;
using UI;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EnemyScripts {
    public class TalkableEnemy: Enemy, ITalkable, IPointerClickHandler {
        [Header("UI panels")]
        [SerializeField] private ShowIndicator battleIndicator;
        [SerializeField] private GameObject enemyPanel;
        
        [Header("Dialogue")]
        public List<DialogueText> dialogue;
        [SerializeField] private DialogueAudioConfig audioConfig;
        
        [Header("Interaction")]
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;
        [SerializeField] private TalkableNPC npcToUnlock;

        private GameObject player;
        private string enemyName;
        
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
            enemyName = dialogue[0].NameText;
        }

        private void Start() {
            if (state == EnemyState.Defeated) {
                if (dialogue.Count > 1) {
                    SetUpNextDialogue();
                }
            }
        }

        private void OnEnable() {
            GameEventsManager.Instance.NPCEvents.OnTalkedToNPC += Unlock;
            GameEventsManager.Instance.DialogueEvents.OnDialogueEnded += ShowPanel;
        }
        
        private void OnDisable() {
            GameEventsManager.Instance.NPCEvents.OnTalkedToNPC -= Unlock;
            GameEventsManager.Instance.DialogueEvents.OnDialogueEnded -= ShowPanel;
        }

        private void Unlock(string npcName) {
            if (npcName == npcToUnlock.GetName()) {
                battleIndicator.ShowIcon();
                ChangeState(EnemyState.Undefeated);
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (state == EnemyState.Locked)
                return;
            
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                Talk(dialogue[0]);
            }
        }
        
        public void Talk(DialogueText dialogueText) {
            DialogueController.Instance.DisplaySentence(dialogueText);
            DialogueController.Instance.SetCurrentAudioConfig(audioConfig);
            battleIndicator.HideIcon();
        }

        private void ShowPanel(string speakerName) {
            if (speakerName.Equals(enemyName)) {
                enemyPanel.SetActive(true);
                enemyPanel.transform.GetChild(0).gameObject.SetActive(true);
                EnemyStateManager.Instance.SetCurrentEnemy(this);
                InputManager.Instance.DisableInput();
            }
        }

        public override void ChangeState(EnemyState state) {
            this.state = state;
            if (state == EnemyState.Defeated) {
                SaveManager.Instance.ChangeEnemyData(enemyID, state);
                foreach (Obstacle obstacle in obstacles) {
                    SaveManager.Instance.ChangeObstacleData(obstacle.GetID(), false);
                }
            }
        }

        private void SetUpNextDialogue() {
            dialogue.Remove(dialogue[0]);
        }
    }
}