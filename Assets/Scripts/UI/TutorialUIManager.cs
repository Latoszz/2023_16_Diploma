using System.Collections;
using Events;
using Tutorial;
using UnityEngine;

namespace UI {
    public class TutorialUIManager : MonoBehaviour {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject inventoryButtonOutline;
        [SerializeField] private GameObject questButton;
        [SerializeField] private GameObject questButtonOutline;

        private bool inventoryUnlocked = false;
        public bool InventoryUnlocked => inventoryUnlocked;
        
        private bool questsUnlocked = false;
        public bool QuestsUnocked => questsUnlocked;

        public static TutorialUIManager Instance;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
        }
        
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory += UnlockInventory;
            GameEventsManager.Instance.TutorialEvents.OnUnlockQuests += UnlockQuests;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory -= UnlockInventory;
            GameEventsManager.Instance.TutorialEvents.OnUnlockQuests -= UnlockQuests;
        }

        private void UnlockInventory() {
            StartCoroutine(EnableInventoryButton());
        }
        
        private IEnumerator EnableInventoryButton() {
            yield return new WaitForSeconds(1);
            StartCoroutine(EnableInventory());
        }

        private IEnumerator EnableInventory() {
            yield return new WaitUntil(() => !TutorialDialogue.Instance.IsOpen);
            inventoryUnlocked = true;
            inventoryButton.SetActive(true);
            inventoryButtonOutline.SetActive(true);
        }
        
        private void UnlockQuests() {
            StartCoroutine(EnableQuestButton());
        }
        
        private IEnumerator EnableQuestButton() {
            yield return new WaitUntil(() => !TutorialDialogue.Instance.IsOpen);
            questsUnlocked = true;
            questButton.SetActive(true);
            questButtonOutline.SetActive(true);
        }
    }
}
