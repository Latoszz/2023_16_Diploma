using Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class GiveItem: MonoBehaviour, IPointerClickHandler {
    [SerializeField] private string itemId;
    private bool hasItem;
    
    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected += ItemCollected;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected -= ItemCollected;
    }

    private void ItemCollected(string itemId) {
        if (itemId == this.itemId) {
            hasItem = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (hasItem) {
            GameEventsManager.Instance.ItemEvents.ItemWithIdGiven(itemId);
            InventoryController.Instance.RemoveItem(itemId);
            hasItem = false;
        }
    }
}
