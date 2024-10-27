using System;
using System.Collections.Generic;
using Audio;
using Interaction;
using ScriptableObjects.Dialogue;
using UnityEngine;

namespace NPC {
    public class TalkableNPC : NPC, ITalkable {
        public List<DialogueText> dialogue;
        [SerializeField] private DialogueController dialogueController;
        [SerializeField] private ShowQuestIndicator questIndicator;
        [SerializeField] private DialogueAudioConfig audioConfig;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;

        private GameObject player;
        private bool talkedTo = false;
        [SerializeField] private string npcID;

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            npcID = Guid.NewGuid().ToString();
        }

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public override void Interact() {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                Talk(dialogue[0]);
                talkedTo = true;
            }
        }

        public void Talk(DialogueText dialogueText) {
            dialogueController.DisplaySentence(dialogueText);
            dialogueController.SetCurrentAudioConfig(audioConfig);
            questIndicator.HideQuestIcon();
        }

        public void SetUpNextDialogue() {
            dialogue.Remove(dialogue[0]);
            questIndicator.ShowQuestIcon();
        }

        public string GetID() {
            return npcID;
        }

        public bool TalkedTo() {
            return talkedTo;
        }
    }
}