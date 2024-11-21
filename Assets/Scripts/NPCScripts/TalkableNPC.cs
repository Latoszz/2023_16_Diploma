using System;
using System.Collections.Generic;
using Audio;
using Events;
using Interfaces;
using UI.Dialogue;
using UI.NPC;
using UnityEngine;

namespace NPCScripts {
    public class TalkableNPC : NPC, ITalkable {
        public List<DialogueText> dialogue;
        [SerializeField] private DialogueController dialogueController;
        [SerializeField] private ShowQuestIndicator questIndicator;
        [SerializeField] private DialogueAudioConfig audioConfig;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;

        private GameObject player;
        private bool talkedTo;
        private string npcName;

        [SerializeField] private string npcID;

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            npcID = Guid.NewGuid().ToString();
        }

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
            npcName = dialogue[0].NameText;
        }

        private void Start() {
            if (!talkedTo) {
                questIndicator.ShowQuestIcon();
            }
        }
        
        public override void Interact() {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                Talk(dialogue[0]);
                talkedTo = true;
                GameEventsManager.Instance.NPCEvents.TalkedToNPC(npcName);
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

        public void SetTalkedTo(bool val) {
            talkedTo = val;
        }
    }
}