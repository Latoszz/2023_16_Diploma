using CardBattles.CardScripts;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using UnityEngine;

namespace CardBattles.Managers {
    public class CardManager : MonoBehaviour {
        public static CardManager Instance;

        [SerializeField]
        private GameObject minionPrefab;
        [SerializeField]
        private GameObject spellPrefab;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }

        public Card CreateCard(CardData cardData, PlayerEnemyMonoBehaviour parentComponent) {
            return CardFactory.CreateCard(cardData, parentComponent, minionPrefab, spellPrefab);
        }
        public CardData LoadCardData(string cardSetName, string cardDataName)
        {
            var cardSet = Resources.Load<CardSetData>("CardSets/"+cardSetName);

            if (cardSet == null)
            {
                Debug.LogError($"CardSet '{cardSetName}' not found in Resources.");
                return null;
            }

            foreach (var card in cardSet.cards) 
            {
                if (card.name == cardDataName)
                {
                    Debug.Log($"Found CardData: {card.name}");
                    return card;
                }
            }

            return null;
        }
        
    }
}