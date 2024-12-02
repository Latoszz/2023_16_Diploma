using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardGamesManager;
using CardBattles.CardScripts;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using CardBattles.Managers.GameSettings;
using NaughtyAttributes;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardBattles.Character {
    public class DeckManager : PlayerEnemyMonoBehaviour {
        [SerializeField] private List<CardSetData> cardSetDatas = new List<CardSetData>();

        private void OnEnable() {
            var loadCards = IsPlayers ? CardGamesLoader.Instance.loadPlayerCards : CardGamesLoader.Instance.loadEnemyCards;
            loadCards.AddListener(SetCardSetData);
        }

        private void OnDisable() {
            var loadCards = IsPlayers ? CardGamesLoader.Instance.loadPlayerCards : CardGamesLoader.Instance.loadEnemyCards;
            loadCards.RemoveListener(SetCardSetData);
        }


        [SerializeField,AllowNesting] 
        private CardSetDictionary cardSets =
            new CardSetDictionary();

        private List<Card> cards = new List<Card>();

        public void AddCard(CardData cardData) {
            var card = CardManager.Instance.CreateCard(cardData,this);
            cards = cards.Prepend(card).ToList();
        }

        public void SetCardSetData(List<CardSetData> loadedCardSetDatas) {
            cardSetDatas = loadedCardSetDatas;
        }

        private IEnumerator Start() {
            yield return new WaitUntil(() => cardSetDatas != null);
            InitializeDeck();
        }

        public void InitializeDeck() {
            CreateCardSetsFromData();
            CreateCardFromDeck();
        }
        
        
        public bool HasCards => cards.Any();

        public Card PopTopCard() {
            if (!HasCards) return null;
            var card = cards[0];
            cards.Remove(card);
            return card;
        }
        
        private void CreateCardSetsFromData() {
            cardSets = new CardSetDictionary();

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
                    var cardSetName = cardSetData.displayName + i;
                    cardSets[cardSetName].Add(card);
                    card.cardSetName = cardSetName;
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
            if(GameStats.Config.resetDeckWhenEmpty)
                InitializeDeck();
            Debug.Log("No more cards honey");
        }

        public List<GameObject> GetCardFromSameCardSet(Card card) {
            if (!cardSets.TryGetValue(card.cardSetName, out var cardsFromSet))
                return new List<GameObject>();
                
            var filteredCards = cardsFromSet.Where(e => e != null && e.gameObject != null);
            return filteredCards.Select(e => e.gameObject).ToList();
        }

        public List<GameObject> GetOtherCardFromSameCardSet(Card card) {
            var output = GetCardFromSameCardSet(card);
            output.Remove(card.gameObject);
            return output;
        }
    }
}