using System;

namespace Events {
    public class ItemEvents {
        public event Action OnItemCollected;
        public event Action<string> OnItemWithIdCollected;

        public event Action<string> OnItemWithIdGiven;
        
        public event Action<string> OnItemReward;
        
        
        public void ItemCollected() {
            OnItemCollected?.Invoke();
        }

        public void ItemWithIdCollected(string itemId) {
            OnItemWithIdCollected?.Invoke(itemId);
        }

        public void ItemWithIdGiven(string itemId) {
            OnItemWithIdGiven?.Invoke(itemId);
        }
        
        public void ItemReward(string itemName) {
            OnItemReward?.Invoke(itemName);
        }
    }
}