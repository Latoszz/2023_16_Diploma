using System;

namespace Events {
    public class ItemEvents {
        public event Action OnItemCollected;
        public event Action<string> OnItemWithIdCollected;

        public event Action<string> OnItemWithIdGiven;
        
        
        public void ItemCollected() {
            OnItemCollected?.Invoke();
        }

        public void ItemWithIdCollected(string itemId) {
            OnItemWithIdCollected?.Invoke(itemId);
        }

        public void ItemWithIdGiven(string itemId) {
            OnItemWithIdGiven?.Invoke(itemId);
        }
    }
}