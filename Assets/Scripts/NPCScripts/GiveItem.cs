using System.Collections.Generic;
using Events;
using UI.Inventory;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NPCScripts {
    public class GiveItem: MonoBehaviour, IPointerClickHandler {
        [SerializeField] private string itemId;

        private bool HasItem() {
            List<CollectibleItemData> allItems = InventoryController.Instance.GetItems();
            foreach (var item in allItems) {
                if (item.itemID == this.itemId)
                    return true;
            }
            return false;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!HasItem()) return;
            GameEventsManager.Instance.ItemEvents.ItemWithIdGiven(itemId);
            InventoryController.Instance.RemoveItem(itemId);
        }
    }
}
