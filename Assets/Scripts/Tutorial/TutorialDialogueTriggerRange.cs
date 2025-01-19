using UI.Dialogue;
using UnityEngine;

namespace Tutorial {
    public class TutorialDialogueTriggerRange: MonoBehaviour {
        [SerializeField] private DialogueText dialogue;
        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player"))
                TutorialDialogue.Instance.DisplaySentence(dialogue);
        }
    }
}