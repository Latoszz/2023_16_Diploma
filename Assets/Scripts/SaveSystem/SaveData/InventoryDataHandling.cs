using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts.CardDatas;
using Esper.ESave;
using UI.Inventory;
using UI.Inventory.Items;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class InventoryDataHandling:MonoBehaviour, ISavable {
        [SerializeField] private GameObject itemList;
        [SerializeField] private GameObject deckList;
        [SerializeField] private GameObject cardSetList;
        
        private List<ItemSlot> allItems = new List<ItemSlot>();
        private List<ItemSlot> allCardSets = new List<ItemSlot>();
        private List<ItemSlot> allDeckCardSets = new List<ItemSlot>();

        private const string ItemSaveID = "Inventory items";
        private const string CardSetSaveID = "Inventory Card Set";
        private const string DeckSaveID = "Inventory deck";

        public static InventoryDataHandling Instance;

        private void Awake() {
            IEnumerable<ItemSlot> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ItemSlot>();
            List<ItemSlot> allItemSlots = new List<ItemSlot>(objects);

            foreach (ItemSlot itemSlot in allItemSlots) {
                if (itemSlot.GetParentName() == itemList.name) {
                    allItems.Add(itemSlot);
                }
                else if (itemSlot.GetParentName() == deckList.name) {
                    allDeckCardSets.Add(itemSlot);
                }
                else {
                    allCardSets.Add(itemSlot);
                }
            }
        }
        
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(ItemSaveID, ItemSaveID);
            foreach (ItemSlot itemSlot in allItems) {
                string itemSlotID = itemSlot.GetID();
                
                if (saveFile.HasData(itemSlotID))
                    saveFile.DeleteData(itemSlotID);
                saveFile.AddOrUpdateData(itemSlotID, itemSlot.IsOccupied());
                
                if (!itemSlot.IsOccupied()) {
                    continue;
                }
                
                CollectibleItem item = (CollectibleItem)itemSlot.GetItem();
                string itemName = item.GetName();
                CollectibleItemData itemData = item.GetItemData();
                string id = itemData.itemID;
                string jsonData = JsonUtility.ToJson(itemData);
                
                saveFile.AddOrUpdateData(itemSlotID + "_item", id);
                saveFile.AddOrUpdateData(id + "_name", itemName);
                saveFile.AddOrUpdateData(id + "_data", jsonData);
            }
            
            PopulateCardSetData(saveFile, allCardSets, CardSetSaveID);
            PopulateCardSetData(saveFile, allDeckCardSets, DeckSaveID);
        }

        private void PopulateCardSetData(SaveFile saveFile, List<ItemSlot> itemSlotList, string saveID) {
            saveFile.AddOrUpdateData(saveID, saveID);
            foreach (ItemSlot itemSlot in itemSlotList) {
                string itemSlotID = itemSlot.GetID();
                
                if (saveFile.HasData(itemSlotID))
                    saveFile.DeleteData(itemSlotID);
                saveFile.AddOrUpdateData(itemSlotID, itemSlot.IsOccupied());
                
                if (!itemSlot.IsOccupied()) {
                    continue;
                }
                
                CardSetItem item = (CardSetItem)itemSlot.GetItem();
                string itemName = item.GetName();
                CardSetData cardSetData = item.GetCardSetData();
                string jsonData = JsonUtility.ToJson(cardSetData);
                
                saveFile.AddOrUpdateData(itemSlotID + "_item", itemName);
                saveFile.AddOrUpdateData(itemName + "_json", jsonData);
            }
        }

        public void LoadSaveData(SaveFile saveFile) {
            if(!saveFile.HasData(ItemSaveID))
                return;
            
            foreach (ItemSlot itemSlot in allItems) {
                string itemSlotID = itemSlot.GetID();
                itemSlot.SetIsOccupied(saveFile.GetData<bool>(itemSlotID));

                if (itemSlot.IsOccupied()) {
                    CollectibleItemData itemData = ScriptableObject.CreateInstance<CollectibleItemData>();
                    string id = saveFile.GetData<string>(itemSlotID + "_item");
                    itemData.itemID = id;
                    JsonUtility.FromJsonOverwrite(saveFile.GetData<string>(id + "_data"), itemData);
                    
                    GameObject itemObject = new GameObject();
                    itemObject.AddComponent<DraggableItem>();
                    CollectibleItem item = itemObject.AddComponent<CollectibleItem>();
                    item.SetItemData(itemData);
                    item.SetSprite(itemData.itemSprite);
                    item.SetName(saveFile.GetData<string>(id + "_name"));
                    
                    itemSlot.AddItem(item);
                    Destroy(itemObject);
                }
            }
            
            LoadCardSetData(saveFile, allCardSets, CardSetSaveID);
            LoadCardSetData(saveFile, allDeckCardSets, DeckSaveID);
        }

        private void LoadCardSetData(SaveFile saveFile, List<ItemSlot> itemSlotList, string saveID) {
            if(!saveFile.HasData(saveID))
                return;
            
            foreach (ItemSlot itemSlot in itemSlotList) {
                string itemSlotID = itemSlot.GetID();
                itemSlot.SetIsOccupied(saveFile.GetData<bool>(itemSlotID));

                if (itemSlot.IsOccupied()) {
                    GameObject itemObject = new GameObject();
                    itemObject.AddComponent<DraggableItem>();
                    CardSetItem item = itemObject.AddComponent<CardSetItem>();
                    string itemName = saveFile.GetData<string>(itemSlotID + "_item");
                    item.SetName(itemName);
                    
                    CardSetData cardSetData = ScriptableObject.CreateInstance<CardSetData>();
                    JsonUtility.FromJsonOverwrite(saveFile.GetData<string>(itemName + "_json"), cardSetData);
                    item.SetCardSetData(cardSetData);
                    item.SetSprite(cardSetData.cardSetIcon);
                    
                    itemSlot.AddItem(item);
                    Destroy(itemObject);
                }
            }
        }
    }
}