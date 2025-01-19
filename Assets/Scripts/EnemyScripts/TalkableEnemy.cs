using System.Collections.Generic;
using Audio;
using Events;
using InputScripts;
using Interfaces;
using NPCScripts;
using UI;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EnemyScripts {
    public class TalkableEnemy: Enemy, ITalkable, IPointerClickHandler {
        [Header("UI panels")]
        [SerializeField] protected ShowIndicator battleIndicator;
        [SerializeField] private GameObject enemyPanel;
        
        [Header("Dialogue")]
        public List<DialogueText> dialogue;
        [SerializeField] private DialogueAudioConfig audioConfig;
        
        [Header("Interaction")]
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;
        [SerializeField] private TalkableNPC npcToUnlock;

        private GameObject player;
        
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start() {
            switch (state) {
                case EnemyState.Defeated: {
                    if (dialogue.Count > 1) {
                        SetUpNextDialogue();
                    }
                    battleIndicator.HideIcon();
                    break;
                }
                case EnemyState.Undefeated:
                    battleIndicator.ShowIcon();
                    break;
                default:
                    battleIndicator.HideIcon();
                    break;
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
            if (state == EnemyState.Defeated)
                return;
            if (npcToUnlock is null)
                return;
            if (npcName == npcToUnlock.GetName()) {
                battleIndicator.ShowIcon();
                ChangeState(EnemyState.Undefeated);
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                Talk(dialogue[0]);
                GameEventsManager.Instance.NPCEvents.TalkedToNPC(gameObject.name);
            }
        }
        
        public void Talk(DialogueText dialogueText) {
            DialogueController.Instance.SetSpeakerID(enemyID);
            DialogueController.Instance.DisplaySentence(dialogueText);
            DialogueController.Instance.SetCurrentAudioConfig(audioConfig);
        }

        private void ShowPanel(string speakerID, DialogueText dialogueText) {
            if (!speakerID.Equals(enemyID)) return;
            if (state == EnemyState.Locked) return;
            enemyPanel.SetActive(true);
            enemyPanel.transform.GetChild(0).gameObject.SetActive(true);
            EnemyStateManager.Instance.SetCurrentEnemy(this);
            InputManager.Instance.DisableAllInput();
        }

        public void SetUpNextDialogue() {
            dialogue.Remove(dialogue[0]);
        }
    }
}