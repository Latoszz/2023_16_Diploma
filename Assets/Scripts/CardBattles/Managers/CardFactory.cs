using CardBattles.CardScripts;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers.GameSettings;
using UnityEngine;

namespace CardBattles.Managers {
    public static class CardFactory {
        public static Card CreateCard(
            CardData cardData,
            PlayerEnemyMonoBehaviour parentComponent,
            GameObject minionPrefab,
            GameObject spellPrefab)
        {
            GameObject cardObject;
            Card cardComponent = null;

            switch (cardData) {
                case MinionData:
                    cardObject = Object.Instantiate(minionPrefab);
                    cardComponent = cardObject.GetComponent<Minion>();
                    
                    
                    
                    break;
                case SpellData:
                    cardObject = Object.Instantiate(spellPrefab);
                    cardComponent = cardObject.GetComponent<Spell>();
                    break;
            }

            if (cardComponent is null) {
                Debug.LogError("Failed to create card.");
                return null;
            }

            cardComponent.Initialize(cardData, parentComponent.IsPlayers);
            cardComponent.transform.SetParent(parentComponent.transform);
            cardComponent.transform.localScale = Vector3.one * CardDisplay.scaleInDeck;
            cardComponent.transform.localPosition = Vector3.zero;

            return cardComponent;
        }
    }
}