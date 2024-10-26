using Events;
using Interfaces;
using UnityEngine.EventSystems;

namespace Items {
    public class CollectibleCardSetItem: CardSetItem, ICollectible, IPointerClickHandler {
        public void Collect() {
            InventoryController.Instance.AddItem(this);
            gameObject.SetActive(false);
            GameEventsManager.Instance.ItemEvents.ItemCollected();
        }

        public void OnPointerClick(PointerEventData eventData) {
            Collect();
        }
    }
}