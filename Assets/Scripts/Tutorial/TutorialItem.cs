using Events;
using UI.Dialogue;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialItem : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject inventoryButtonOutline;
        [SerializeField] private DialogueText dialogueText;
        private Item item;

        private void Awake() {
            item = GetComponent<CollectibleCardSetItem>();
        }

        private void Update() {
            item.enabled = TutorialDialogue.Instance.CurrentDialogue == dialogueText;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (TutorialDialogue.Instance.CurrentDialogue != dialogueText) {
                return;
            }
            TutorialDialogue.Instance.DisplayNextSentence();
            GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
            inventoryButton.SetActive(true);
            inventoryButtonOutline.SetActive(true);
        }
    }
}
