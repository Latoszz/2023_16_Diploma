using System.Collections.Generic;
using Audio;
using Esper.ESave;
using Events;
using Interfaces;
using SaveSystem;
using UI;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction {
    public class DialogueTrigger : MonoBehaviour, ITalkable, IPointerClickHandler, ISavable {
        [SerializeField] private List<DialogueText> dialogue;
        [SerializeField] private ShowIndicator indicator;
        [SerializeField] private DialogueAudioConfig audioConfig;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;
        private GameObject player;
        private bool talkedTo;

        private const string DoorDialogueSaveID = "DoorDialogueSaveID";
        
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start() {
            if (!talkedTo) {
                indicator.ShowIcon();
            }
            else {
                this.enabled = false;
            }
        }
    
        public void OnPointerClick(PointerEventData eventData) {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                Talk(dialogue[0]);
                GameEventsManager.Instance.NPCEvents.TalkedToNPC(name);
            }
        }
    
        public void Talk(DialogueText dialogueText) {
            DialogueController.Instance.SetSpeakerID(name);
            DialogueController.Instance.DisplaySentence(dialogueText);
            DialogueController.Instance.SetCurrentAudioConfig(audioConfig);
            indicator.HideIcon();
        }

        public void SetUpNextDialogue() {
            dialogue.Remove(dialogue[0]);
            indicator.ShowIcon();
        }

        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(DoorDialogueSaveID, talkedTo);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (saveFile.HasData(DoorDialogueSaveID)) {
                talkedTo= saveFile.GetData<bool>(DoorDialogueSaveID);
            }
        }
    }
}
