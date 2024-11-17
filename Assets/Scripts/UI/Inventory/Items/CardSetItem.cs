
using CardBattles.CardScripts.CardDatas;
using UnityEngine;

public class CardSetItem : Item {
    [SerializeField] private CardSetData cardSetData;
    private bool unlocked;

    private void Awake() {
        if (cardSetData != null) {
            sprite = cardSetData.cardSetIcon;
            itemName = cardSetData.name;
        }
    }

    public CardSetData GetCardSetData() {
        return cardSetData;
    }
    
    public void SetCardSetData(CardSetData cardSetData) {
        this.cardSetData = cardSetData;
    }
}
