using System;
using System.Collections.Generic;
using Audio;
using Events;
using Interfaces;
using UI;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPCScripts {
    public class TalkableNPC : NPC, ITalkable {
        public List<DialogueText> dialogue;
        [FormerlySerializedAs("questIndicator")] [SerializeField] private ShowIndicator indicator;
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
                indicator.ShowIcon();
                indicator.HideName();
            }
            else {
                indicator.ShowName();
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
            DialogueController.Instance.DisplaySentence(dialogueText);
            DialogueController.Instance.SetCurrentAudioConfig(audioConfig);
            indicator.HideIcon();
        }

        public void SetUpNextDialogue() {
            dialogue.Remove(dialogue[0]);
            indicator.ShowIcon();
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

        public string GetName() {
            return npcName;
        }
    }
}