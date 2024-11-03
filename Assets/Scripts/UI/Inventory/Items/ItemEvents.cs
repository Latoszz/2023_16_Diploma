using System;

namespace Items {
    public class ItemEvents {
        public event Action OnItemCollected;
        public event Action<string> OnItemWithIdCollected;
        
        public void ItemCollected() {
            OnItemCollected?.Invoke();
        }

        public void ItemWithIdCollected(string itemId) {
            OnItemWithIdCollected?.Invoke(itemId);
        }
    }
}