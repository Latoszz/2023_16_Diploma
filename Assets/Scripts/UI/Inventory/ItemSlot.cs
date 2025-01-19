using System;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IDropHandler {
        [SerializeField] private GameObject selectedShader;
        [SerializeField] private GameObject itemList;
        [SerializeField] private GameObject deckList;
        [SerializeField] private GameObject cardSetList;
        [SerializeField] private GameObject itemObjectPrefab;

        [SerializeField] private string itemSlotID;
        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            itemSlotID = Guid.NewGuid().ToString();
        }

        private InventoryController inventoryController;
        private string parentName;
        private Item item;
        private GameObject itemObject;
        private bool isOccupied = false;

        private void Awake() {
            parentName = transform.parent.name;
            inventoryController = InventoryController.Instance;
        }
    
        public void AddItem(Item item) {
            this.item = item;
            isOccupied = true;
            CreateItemObject();
        }
        private void CreateItemObject() {
            itemObject = Instantiate(itemObjectPrefab, transform);
            itemObject.transform.SetParent(transform);
            itemObject.GetComponent<Image>().sprite = item.GetSprite();
            itemObject.GetComponent<DraggableItem>().SetItemData(item);
        }

        public bool IsOccupied() {
            return isOccupied;
        }

        public void SetIsOccupied(bool value) {
            isOccupied = value;
        }

        public GameObject GetSelectedShader() {
            return selectedShader;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) 
                return;
        
            if (selectedShader.activeSelf) {
                inventoryController.DeselectAllSlots();
                if (item is CardSetItem)
                    inventoryController.HideCardSetDetails();
                return;
            }
            SelectSlot();
        }

        private void SelectSlot() {
            inventoryController.DeselectAllSlots();
            selectedShader.SetActive(true);
            if (item is CardSetItem cardSet) {
                if (inventoryController.IsCardSetDetailsOpen())
                    inventoryController.HideCardSetDetails();
                inventoryController.ShowCardSetDetails(cardSet.GetCardSetData());
            }
            else if (item is CollectibleItem collectibleItem) {
                if (collectibleItem.GetItemData().itemID.Contains("Note")) {
                    inventoryController.OpenNotePanel(collectibleItem.GetItemData().text);
                }
            }
        }

        public void OnDrop(PointerEventData eventData) {
            if (!isOccupied) {
                GameObject itemObject = eventData.pointerDrag;
                DraggableItem draggableItem = itemObject.GetComponent<DraggableItem>();
                if (draggableItem is null)
                    return;

                if (parentName == itemList.name && draggableItem.GetItemData() is CardSetItem) {
                    return;
                }

                if ((parentName == cardSetList.name || parentName == deckList.name) &&
                    draggableItem.GetItemData() is CollectibleItem) {
                    return;
                }
            
                ItemSlot previousItemSlot = draggableItem.GetParent().GetComponent<ItemSlot>();
                previousItemSlot.SetIsOccupied(false);
                previousItemSlot.ClearItem();
            
                draggableItem.SetParentAfterDrag(transform);
                SetIsOccupied(true);
                SetItem(draggableItem.GetItemData());
            }
        }

        public Item GetItem() {
            return item;
        }

        private void SetItem(Item newItem) {
            item = newItem;
        }
    
        private void ClearItem() {
            item = null;
        }

        public void RemoveItem() {
            item = null;
            Destroy(itemObject);
        }

        public string GetParentName() {
            return parentName ??= transform.parent.name;
        }
    
        public string GetID() {
            return itemSlotID;
        }
    }
}
