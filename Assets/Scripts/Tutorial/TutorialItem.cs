using System.Collections;
using Events;
using InputScripts;
using UI.Dialogue;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialItem : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private DialogueText dialogueText;
        [SerializeField] private Item item;

        private void Update() {
            item.enabled = TutorialDialogue.Instance.CurrentDialogue == dialogueText;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (TutorialDialogue.Instance.CurrentDialogue != dialogueText) {
                return;
            }
            TutorialDialogue.Instance.DisplayNextSentence();
            InputManager.Instance.EnableInventory();
            GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
            GameEventsManager.Instance.TutorialEvents.UnlockStatue();
            GameEventsManager.Instance.TutorialEvents.UnlockInventory();
        }
    }
}
