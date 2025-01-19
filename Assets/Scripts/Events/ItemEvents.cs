using System;
using UI.Inventory.Items;

namespace Events {
    public class ItemEvents {
        public event Action OnItemCollected;
        public event Action<string> OnItemWithIdCollected;

        public event Action<Item> OnItemCollectedItem;

        public event Action<string> OnItemWithIdGiven;
        
        public event Action<string> OnItemReward;
        
        
        public void ItemCollected() {
            OnItemCollected?.Invoke();
        }

        public void ItemWithIdCollected(string itemId) {
            OnItemWithIdCollected?.Invoke(itemId);
        }
        
        public void ItemCollectedItem(Item item) {
            OnItemCollectedItem?.Invoke(item);
        }

        public void ItemWithIdGiven(string itemId) {
            OnItemWithIdGiven?.Invoke(itemId);
        }
        
        public void ItemReward(string itemName) {
            OnItemReward?.Invoke(itemName);
        }
    }
}