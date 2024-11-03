using System.Collections.Generic;

public class InventorySaveDataOld : SaveDataOld{
    [System.Serializable]
    public struct ItemData {
        public int index;
        public string name;
        public string image;
    }
    
    public struct CardSetItemData {
        public int index;
        public string name;
        public string image;
        public string cardSetData;
    }

    public List<ItemData> itemDatas = new List<ItemData>();
    public List<CardSetItemData> deckDatas = new List<CardSetItemData>();
    public List<CardSetItemData> cardSetDatas = new List<CardSetItemData>();
}