using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Infos;
using UI.Inventory;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class Removable : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private CollectibleItemData itemData;
    [Range(0, 10)] [SerializeField] private int detectionDistance;
    [SerializeField] private Popup popup;
    [SerializeField] private string textToDisplay;

    private GameObject player;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!(Vector3.Distance(player.transform.position, gameObject.transform.position) <= detectionDistance)) return;
        if (CheckInventory()) {
            gameObject.SetActive(false);
        }
        else {
            popup.OpenPopup(textToDisplay);
        }
    }

    private bool CheckInventory() {
        List<CollectibleItemData> allItems = InventoryController.Instance.GetItems();
        foreach (var item in allItems) {
            if (item.itemID == itemData.itemID) {
                return true;
            }
        }
        return false;
    }
}
