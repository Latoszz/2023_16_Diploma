using UI.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialDialogueTrigger: MonoBehaviour, IPointerClickHandler {
        [SerializeField] private DialogueText dialogue;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;
        
        public void OnPointerClick(PointerEventData eventData) {
            TutorialDialogue.Instance.DisplaySentence(dialogue);
        }
    }
}