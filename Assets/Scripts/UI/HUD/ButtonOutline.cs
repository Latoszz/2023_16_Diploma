using Events;
using Tutorial;
using UI.Dialogue;
using UI.Inventory;
using UnityEngine;

namespace UI.HUD {
    public class ButtonOutline : MonoBehaviour {
        [SerializeField] private GameObject inventoryOutline;
        [SerializeField] private GameObject questOutline;

        private bool inventoryUnlocked;
        private bool questUnlocked;
        private bool inventoryClicked;
        private bool questClicked;

        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnButtonClicked += ChangeBool;
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory += InventoryUnlock;
            GameEventsManager.Instance.TutorialEvents.OnUnlockQuests += QuestUnlock;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnButtonClicked -= ChangeBool;
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory -= InventoryUnlock;
            GameEventsManager.Instance.TutorialEvents.OnUnlockQuests -= QuestUnlock;
        }

        private void ChangeBool(string buttonName) {
            if (buttonName.Contains("Inventory")) {
                inventoryClicked = true;
            }
            
            else if (buttonName.Contains("Quest")) {
                questClicked = true;
            }
        }

        private void InventoryUnlock() {
            inventoryUnlocked = true;
        }

        private void QuestUnlock() {
            questUnlocked = true;
        }
        
        private void Update() {
            if (inventoryClicked)
                inventoryOutline.SetActive(false);

            if (questClicked) {
                questOutline.SetActive(false);
            }
            
            if (InventoryController.Instance.IsOpen()) {
                inventoryOutline.SetActive(false);
                questOutline.SetActive(false);
            }

            if (!InventoryController.Instance.IsOpen()) {
                if (!inventoryClicked && inventoryUnlocked) {
                    inventoryOutline.SetActive(true);
                }

                if (!questClicked && questUnlocked) {
                    questOutline.SetActive(true);
                }
            }

            if (!DialogueController.Instance.DialogueClosed || TutorialDialogue.Instance.IsOpen) {
                inventoryOutline.SetActive(false);
                questOutline.SetActive(false);
            }
            
            else if (DialogueController.Instance.DialogueClosed || !TutorialDialogue.Instance.IsOpen) {
                if (InventoryController.Instance.IsOpen())
                    return;
                if (!inventoryClicked && inventoryUnlocked) {
                    inventoryOutline.SetActive(true);
                }

                if (!questClicked && questUnlocked) {
                    questOutline.SetActive(true);
                }
            }
        }
    }
}
