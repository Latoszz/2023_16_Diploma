using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private Item itemData;
    private Transform parentAfterDrag;
    
    private Image itemImage;
    private LayoutElement itemLayoutElement;

    private void Awake() {
        itemLayoutElement = transform.gameObject.GetComponent<LayoutElement>();
        itemImage = GetComponent<Image>();
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(GameObject.Find("Inventory UI").transform);
        itemLayoutElement.ignoreLayout = true;
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemLayoutElement.ignoreLayout = false;
        transform.SetParent(parentAfterDrag);
        itemImage.raycastTarget = true;
    }

    public void SetParentAfterDrag(Transform newParent) {
        parentAfterDrag = newParent;
    }

    public Transform GetParent() {
        return parentAfterDrag;
    }

    public void SetItemData(Item item) {
        itemData = item;
    }

    public Item GetItemData() {
        return itemData;
    }
}
