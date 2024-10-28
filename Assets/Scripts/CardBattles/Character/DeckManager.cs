using System;
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardBattles.Character {
    public class DeckManager : PlayerEnemyMonoBehaviour {
        [SerializeField] private List<CardSetData> cardSetDatas = new List<CardSetData>();

        
        //TODO ADD SERIAZABLE DICTIONARY
        [SerializeField]
        private SerializableDictionary<string, List<Card>> cardSets =
            new SerializableDictionary<string, List<Card>>();

        public List<Card> cards = new List<Card>();
        
        private void Start() {
            
            /*
            if (!Application.isEditor)
                cardSetDatas = LoadCardSetData();
                */

            if (cardSetDatas == null) {
                Debug.LogError("cardSetDatas is null");
            }
            
            InitializeDeck();
        }

        public void InitializeDeck() {
            CreateCardSetsFromData(); 
            CreateCardFromDeck();
        }
        
        
        private List<CardSetData> LoadCardSetData() {
            if (IsPlayers)
                return InventoryController.Instance.GetDeck();
            return EnemyStateManager.Instance.GetCurrentEnemy().GetDeck();
        }
        

        private void CreateCardSetsFromData() {
            int i = 0;
            foreach (var cardSetData in cardSetDatas) {
                i++; 
                
                
                if (cardSetData == null) {
                    Debug.LogError("cardSetData is null at index ");
                    continue;
                }

                if (cardSetData.cards == null) {
                    Debug.LogError("cardSetData.cards is null for cardSetData: " + cardSetData.displayName);
                    continue;
                }

                foreach (var cardData in cardSetData.cards) {
                    if (cardData == null) {
                        Debug.LogError("cardData is null in cardSetData: " + cardSetData.displayName);
                        continue;
                    }

                    var card = CardManager.Instance.CreateCard
                    (cardData, this); 

                    if (card == null) {
                        Debug.LogError("Card creation failed for cardData in cardSetData: " + cardSetData.displayName);
                        continue;
                    }

                    cardSets.TryAdd(cardSetData.displayName + i, new List<Card>());
                    cardSets[cardSetData.displayName + i].Add(card);
                }
            }
        }


        private void CreateCardFromDeck() {
            var cardLists = cardSets.Values.ToList();
            var allCards = new List<Card>();
            foreach (var _ in cardLists) {
                allCards.AddRange(_);
            }

            var shuffledList = allCards.OrderBy(_ => Guid.NewGuid()).ToList(); //randomly shuffles

            cards.AddRange(shuffledList);
        }

        public void NoMoreCards() {
            //TODO ADD SOME ANIMATION
            Debug.Log("No more cards honey");
        }

        public List<GameObject> GetCardFromSameCardSet(Card card) {
            foreach (var cardSet in cardSets.Values) {
                
                if (cardSet.Contains(card)) {
                    return cardSet.Select(e => e.gameObject).ToList();
                }
            }
            return new List<GameObject>();
        }
    }
}