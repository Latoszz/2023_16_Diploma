using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IDropHandler {
    [SerializeField] private GameObject selectedShader;
    [SerializeField] private GameObject itemList;
    [SerializeField] private GameObject deckList;
    [SerializeField] private GameObject cardSetList;

    [SerializeField] private string itemSlotID;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        itemSlotID = Guid.NewGuid().ToString();
    }

    private string parentName;
    private Item item;
    private bool isOccupied = false;
    private bool isActive;

    private void Awake() {
        parentName = transform.parent.name;
    }
    
    public void AddItem(Item item) {
        this.item = item;
        isOccupied = true;
        CreateItemObject();
    }
    private void CreateItemObject() {
        GameObject itemObject = new GameObject();
        itemObject.transform.SetParent(transform);
        
        RectTransform itemRectTransform = itemObject.AddComponent<RectTransform>();
        itemRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        itemRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        itemRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        itemRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        itemRectTransform.anchorMin = new Vector2(0, 0);
        itemRectTransform.anchorMax = new Vector2(1, 1);
        itemRectTransform.localScale = new Vector3(1, 1, 1);
        
        Image objectImage = itemObject.AddComponent<Image>();
        objectImage.sprite = item.GetSprite();

        itemObject.AddComponent<DraggableItem>().SetItemData(item);
        itemObject.AddComponent<LayoutElement>();
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
        if (eventData.button == PointerEventData.InputButton.Left)
            SelectSlot();
    }

    private void SelectSlot() {
        InventoryController.Instance.DeselectAllSlots();
        selectedShader.SetActive(true);
        if (item is CardSetItem cardSet) {
            InventoryController.Instance.ShowCardSetDetails(cardSet.GetCardSetData());
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isOccupied) {
            GameObject itemObject = eventData.pointerDrag;
            DraggableItem draggableItem = itemObject.GetComponent<DraggableItem>();

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

    public string GetParentName() {
        return parentName ??= transform.parent.name;
    }
    
    public string GetID() {
        return itemSlotID;
    }
}
