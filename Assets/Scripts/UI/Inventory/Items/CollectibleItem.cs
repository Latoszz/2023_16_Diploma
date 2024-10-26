using Events;
using Interfaces;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectibleItem : Item, ICollectible, IPointerClickHandler {
    [SerializeField] private CollectibleItemData itemData;

    public CollectibleItemData GetItemData() {
        return itemData;
    }
    
    public void SetItemData(CollectibleItemData itemData) {
        this.itemData = itemData;
    }

    public void Collect() {
        InventoryController.Instance.AddItem(this);
        gameObject.SetActive(false);
        GameEventsManager.Instance.ItemEvents.ItemCollected();
    }

    public void OnPointerClick(PointerEventData eventData) {
        Collect();
    }
}