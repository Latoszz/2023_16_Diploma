using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class InventoryController : MonoBehaviour {
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private ManageCardSetDetails manageCardSetDetails;
    [SerializeField] private InputAction rightClickAction;

    [SerializeField] private List<ItemSlot> itemSlots;
    [SerializeField] private List<ItemSlot> cardSetSlots;
    [SerializeField] private List<ItemSlot> deckSlots;
    
    private PostProcessVolume postProcessVolume;

    private bool isOpen;
    
    public static InventoryController Instance = null; 

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }         
        else if (Instance != this) {
            Destroy(gameObject);
        }          
        //DontDestroyOnLoad(gameObject); 
        if (inventoryUI == null)
            inventoryUI = GameObject.FindWithTag("Inventory UI");
        postProcessVolume = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
    }

    private void OnEnable() {
        rightClickAction.Enable();
        rightClickAction.performed += ToggleInventory;
    }

    private void OnDisable() {
        rightClickAction.performed -= ToggleInventory;
        rightClickAction.Disable();
    }

    private void ToggleInventory(InputAction.CallbackContext context) {
        if (PauseManager.Instance.IsOpen)
            return;
        if(isOpen)
            HideInventory();
        else {
            ShowInventory();
        }
    }
    
    public void ShowInventory() {
        if (PauseManager.Instance.IsOpen)
            return;
        inventoryUI.SetActive(true);
        postProcessVolume.enabled = true;
        isOpen = true;
        HUDController.Instance.HideHUD();
        InputManager.Instance.DisableInput();
    }

    public void HideInventory() {
        postProcessVolume.enabled = false;
        manageCardSetDetails.Hide();
        DeselectAllSlots();
        inventoryUI.SetActive(false);
        isOpen = false;
        HUDController.Instance.ShowHUD();
        InputManager.Instance.EnableInput();
    }

    public void AddItem(Item item) {
        if (item is CardSetItem)
            AddToSlot(item, cardSetSlots);
        else
            AddToSlot(item, itemSlots);
    }

    private void AddToSlot(Item item, List<ItemSlot> itemList) {
        foreach (ItemSlot itemSlot in itemList) {
            if (!itemSlot.IsOccupied()) {
                itemSlot.AddItem(item);
                itemSlot.SetIsOccupied(true);
                return;
            }
        }
    }

    public void DeselectAllSlots() {
        DeselectSlots(itemSlots);
        DeselectSlots(cardSetSlots);
        DeselectSlots(deckSlots);
    }

    private void DeselectSlots(List<ItemSlot> itemList) {
        foreach (ItemSlot itemSlot in itemList) {
            itemSlot.GetSelectedShader().SetActive(false);
        }
    }

    public void ShowCardSetDetails(CardSetData cardSetData) {
        if (!manageCardSetDetails.IsOpen)
            manageCardSetDetails.ReadCardSet(cardSetData);
    }

    public List<ItemSlot> GetDeckSlots() {
        return deckSlots;
    }
    
    public List<ItemSlot> GetCardSetSlots() {
        return cardSetSlots;
    }
    
    public List<ItemSlot> GetItemSlots() {
        return itemSlots;
    }

    public List<CardSetData> GetDeck() {
        List<CardSetData> cardSets = new List<CardSetData>();
        foreach(ItemSlot slot in deckSlots)
            cardSets.Add(((CardSetItem)slot.GetItem()).GetCardSetData());
        return cardSets;
    }
    
    public List<CardSetData> GetCardSets() {
        List<CardSetData> cardSets = new List<CardSetData>();
        foreach(ItemSlot slot in cardSetSlots)
            cardSets.Add(((CardSetItem)slot.GetItem()).GetCardSetData());
        return cardSets;
    }
    
    public List<CollectibleItemData> GetItems() {
        List<CollectibleItemData> items = new List<CollectibleItemData>();
        foreach(ItemSlot slot in itemSlots)
            items.Add(((CollectibleItem)slot.GetItem()).GetItemData());
        return items;
    }

    public bool IsOpen() {
        return isOpen;
    }
}
