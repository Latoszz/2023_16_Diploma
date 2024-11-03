using CardBattles.CardScripts;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using UnityEngine;

namespace CardBattles.Managers {
    public class CardManager : MonoBehaviour {
        public static CardManager Instance;

        public GameObject minionPrefab;
        public GameObject spellPrefab;

        private void Awake() {
            if (Instance is null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        public Card CreateCard(CardData cardData, PlayerEnemyMonoBehaviour parentComponent) {
            GameObject cardObject;
            Card cardComponent = null;

            switch (cardData) {
                case MinionData:
                    cardObject = Instantiate(minionPrefab);
                    cardComponent = cardObject.GetComponent<Minion>();
                    break;
                case SpellData:
                    cardObject = Instantiate(spellPrefab);
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