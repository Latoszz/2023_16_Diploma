using Tutorial;
using UI.Infos;
using UI.Inventory;
using UnityEngine;

namespace UI.HUD {
    public class ButtonOutline : MonoBehaviour {
        [SerializeField] private GameObject inventoryOutline;
        [SerializeField] private GameObject questOutline;
        
        private void Update() {
            if (InventoryController.Instance.IsOpen()) {
                inventoryOutline.SetActive(false);
            }

            if (QuestListPanel.Instance.IsOpen) {
                questOutline.SetActive(false);
            }

            if (TutorialDialogue.Instance.IsOpen) {
                inventoryOutline.SetActive(false);
                questOutline.SetActive(false);
            }
        }
    }
}
