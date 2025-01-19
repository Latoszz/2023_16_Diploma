using CardBattles.CardScripts;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using UnityEngine;

namespace CardBattles.Mydebug {
    public class CardCreator : MonoBehaviour {
        [SerializeField]
        private GameObject minionPrefab;
        [SerializeField]
        private GameObject spellPrefab;
        public PlayerEnemyMonoBehaviour parentComponent;
        public CardData cardData;

        public Card CreateCardInEditor() {
            return CardFactory.CreateCard(cardData, parentComponent, minionPrefab, spellPrefab);
        }
        public Card CreateCardInEditor(CardData givenCardData, PlayerEnemyMonoBehaviour givenParentComponent) {
            return CardFactory.CreateCard(givenCardData, givenParentComponent, minionPrefab, spellPrefab);
        }
    }
}