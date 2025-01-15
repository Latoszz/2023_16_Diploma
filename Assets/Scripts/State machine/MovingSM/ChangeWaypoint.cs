using System.Collections.Generic;
using Events;
using UI.Dialogue;
using UnityEngine;

namespace State_machine.MovingSM {
    public class ChangeWaypoint : MonoBehaviour {
        [SerializeField] private MovingSM movingSM;
        [SerializeField] private List<Transform> newWaypoints;
        [SerializeField] private string id;
        [SerializeField] private DialogueText dialogueTextToStart;
        [SerializeField] private DialogueText dialogueTextToStop;

        private List<Transform> oldWaypoints;

        private void Start() {
            oldWaypoints = movingSM.GetWaypoints();
        }

        private void OnEnable() {
            GameEventsManager.Instance.DialogueEvents.OnDialogueEnded += ToggleFollow;
        }

        private void OnDisable() {
            GameEventsManager.Instance.DialogueEvents.OnDialogueEnded -= ToggleFollow;
        }

        private void ToggleFollow(string speakerId, DialogueText dialogueText) {
            if (speakerId != id) return;
            if (dialogueText == dialogueTextToStart){
                movingSM.SetWaypoints(newWaypoints);
                movingSM.ResetWaypointIndex();
            }
            else if (dialogueText == dialogueTextToStop) {
                movingSM.SetWaypoints(oldWaypoints);
            }
        }
    
    }
}
