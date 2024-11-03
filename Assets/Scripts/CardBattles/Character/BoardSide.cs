#nullable enable
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.Character {
    public class BoardSide : PlayerEnemyMonoBehaviour {
        [SerializeField] public CardSpot[] cardSpots = new CardSpot[4];

        [SerializeField, Required]
        public Hero.Hero hero = null!;


        private void Start() {
            GetCardSpots();
        }

        private void GetCardSpots() {
            cardSpots = GetComponentsInChildren<CardSpot>();
        }

        public List<CardSpot> GetEmptyCardSpots() {
            return cardSpots.Where(e => e.card is null).ToList();
        }


        public List<IDamageable?> GetIDamageables() {
            List<IDamageable?> output = new List<IDamageable?>();
            var cards = cardSpots.Select(e => e.card).ToList();
            if (cards.Count != cardSpots.Length) {
                Debug.LogError(":(");
            }

            foreach (var card in cards) {
                if (card is not IDamageable damageable)
                    output.Add(hero);
                else {
                    output.Add(damageable);
                }
            }

            if (output.Count != cardSpots.Length) {
                Debug.LogError(">:(");
            }

            return output;
        }


        //TODO A BUG WHERE IF FIRST IS NULL IT DOESNT PICK IT UP
        public List<IAttacker?> GetIAttackers() {
            List<IAttacker?> output = new List<IAttacker?>();

            var cards = cardSpots.Select(e => e?.card).ToList();
            if (cards.Count != cardSpots.Length) {
                Debug.LogError(":(");
            }

            foreach (var card in cards) {
                if (card is not IAttacker attacker) {
                    output.Add(null);
                }
                else {
                    output.Add(attacker);
                }
            }

            return output;
        }

        public List<GameObject> GetNoNullCardsObjects() {
            List<GameObject> output = new List<GameObject>();
            var cards = cardSpots.Select(e => e.card).ToList();
            foreach (var card in cards) {
                if (card is not null) {
                    output.Add(card.gameObject);
                }
            }

            return output;
        }

        public List<Card> GetNoNullCards() {
            List<Card> output = new List<Card>();
            var cards = cardSpots.Select(e => e.card).ToList();
            foreach (var card in cards) {
                if (card is not null) {
                    output.Add(card);
                }
            }

            return output;
        }

        public List<GameObject> GetAdjecentCards(Card card) {
            List<GameObject> output = new List<GameObject>();
            for (int i = 0; i < cardSpots.Length; i++) {
                if (cardSpots[i].card == card) {
                    if (i < 3)
                        if (cardSpots[i + 1].card is not null)
                            output.Add(cardSpots[i + 1].card.gameObject);
                    if (i > 0)
                        if (cardSpots[i - 1].card is not null)
                            output.Add(cardSpots[i - 1].card.gameObject);
                }
            }

            return output;
        }
    }
}