using System;
using Events;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectibleItem : Item, ICollectible, IPointerClickHandler {
    [SerializeField] private CollectibleItemData itemData;
    [SerializeField] private string itemID;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        itemID = Guid.NewGuid().ToString();
    }
    
    private bool collected  = false;

    public CollectibleItemData GetItemData() {
        return itemData;
    }
    
    public void SetItemData(CollectibleItemData itemData) {
        this.itemData = itemData;
    }

    public void Collect() {
        InventoryController.Instance.AddItem(this);
        collected = true;
        gameObject.SetActive(false);
        GameEventsManager.Instance.ItemEvents.ItemWithIdCollected(itemName);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Collect();
    }

    public string GetID() {
        return itemID;
    }

    public bool IsCollected() {
        return collected;
    }

    public void SetCollected(bool value) {
        collected = value;
        gameObject.SetActive(!collected);
    }
}