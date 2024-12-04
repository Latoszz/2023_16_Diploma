using System.Collections;
using Events;
using Tutorial;
using UnityEngine;

namespace UI {
    public class TutorialUIManager : MonoBehaviour {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject inventoryButtonOutline;
        
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory += UnlockInventory;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockInventory -= UnlockInventory;
        }

        private void UnlockInventory() {
            StartCoroutine(EnableInventoryButton());
        }
        
        private IEnumerator EnableInventoryButton() {
            yield return new WaitUntil(() => !TutorialDialogue.Instance.IsOpen);
            inventoryButton.SetActive(true);
            inventoryButtonOutline.SetActive(true);
        }
    }
}
