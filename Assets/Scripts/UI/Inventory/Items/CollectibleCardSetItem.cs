using System;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory.Items {
    public class CollectibleCardSetItem: CardSetItem, ICollectible, IPointerClickHandler {
        [SerializeField] private string itemID;
        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            itemID = Guid.NewGuid().ToString();
        }
        
        private bool collected  = false;
        
        public void Collect() {
            InventoryController.Instance.AddItem(this);
            collected = true;
            gameObject.SetActive(false);
            GameEventsManager.Instance.ItemEvents.ItemCollected();
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
}