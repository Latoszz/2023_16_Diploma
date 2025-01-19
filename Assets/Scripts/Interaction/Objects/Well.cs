using System.Collections.Generic;
using UI.Infos;
using UI.Inventory;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Well : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private CollectibleItemData requiredItem;
        [Range(0, 10)] [SerializeField] private float detectionDistance;
        [SerializeField] private Popup popup;
        [SerializeField] private string textToDisplay;

        private GameObject player;
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (!(Vector3.Distance(player.transform.position, gameObject.transform.position) <= detectionDistance)) return;
            if (CheckInventory()) {
                
            }
            else {
                popup.OpenPopup(textToDisplay);
            }
        }
        
        private bool CheckInventory() {
            List<CollectibleItemData> allItems = InventoryController.Instance.GetItems();
            foreach (var item in allItems) {
                if (item.itemID == requiredItem.itemID) {
                    return true;
                }
            }
            return false;
        }
    }
}
