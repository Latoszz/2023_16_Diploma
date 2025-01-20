using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using CardBattles.CardScripts;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Character.Mana;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace CardBattles.Character {
    public class CharacterManager : PlayerEnemyMonoBehaviour {
        [Header("Data")] [SerializeField]
        public HandManager hand;

        [SerializeField] public DeckManager deck;
        [SerializeField] public Hero.Hero hero;
        [SerializeField] public BoardSide boardSide;
        [SerializeField] public ManaManager manaManager;

        [ShowNativeProperty]
        public bool IsYourTurn {
            get {
                if (Application.isPlaying)
                    return TurnManager.Instance.isPlayersTurn == IsPlayers;
                return false;
            }
        }
        
        private IEnumerator ForceAddCardsToHandCoroutine(CardData cardData, int amount) {
            for (int i = 0; i < amount; i++) {
                deck.AddCard(cardData);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Draw(amount, 0));
        }
        private IEnumerator AddToEndOfDeck(CardData cardData) {
            deck.AddCardToEnd(cardData);
            yield return null;
        }
        

        
        private static UnityEvent<Card, ICardPlayTarget, bool> onCardPlayed =
            new UnityEvent<Card, ICardPlayTarget, bool>();

        private static UnityEvent<PlayerEnemyMonoBehaviour, IHasCost, int> onDrawCard =
            new UnityEvent<PlayerEnemyMonoBehaviour, IHasCost, int>();

        private static UnityEvent<PlayerEnemyMonoBehaviour, CardData, int> onForceAddToHand =
            new UnityEvent<PlayerEnemyMonoBehaviour, CardData, int>();
        private static UnityEvent<PlayerEnemyMonoBehaviour, CardData> onForceBottomDeck =
            new UnityEvent<PlayerEnemyMonoBehaviour, CardData>();

        private void Awake() {
            onCardPlayed.AddListener(OnCardPlayedHandler);
            onDrawCard.AddListener(OnDrawCardHandler);
            onForceAddToHand.AddListener(OnForceAddToHandHandler);
            onForceBottomDeck.AddListener(OnForceBottomDeckHandler);

        }

        private void OnDestroy() {
            onCardPlayed.RemoveListener(OnCardPlayedHandler);
            onDrawCard.RemoveListener(OnDrawCardHandler);
            onForceAddToHand.RemoveListener(OnForceAddToHandHandler);
            onForceBottomDeck.AddListener(OnForceBottomDeckHandler);
        }

        public static void SummonACard(Card card, ICardPlayTarget target) {
            onCardPlayed?.Invoke(card, target, true);
        }

        public static void PlayACard(Card card, ICardPlayTarget target) {
            onCardPlayed?.Invoke(card, target, false);
        }

        public static void DrawACard(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, IHasCost iHasCost,int amount = 1) {
            onDrawCard?.Invoke(playerEnemyMonoBehaviour, iHasCost, amount);
        }

        public static void DrawACard(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, int cost = 1, int amount = 1) {
            onDrawCard?.Invoke(playerEnemyMonoBehaviour, new HasCost(cost), amount);
        }
        public static void AddCardsToHand(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, CardData cardData, int amount) {
            onForceAddToHand?.Invoke(playerEnemyMonoBehaviour, cardData,amount);
        }
        public static void ForceBottomDeck(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, CardData cardData) {
            onForceBottomDeck?.Invoke(playerEnemyMonoBehaviour, cardData);
        }
        private void OnForceBottomDeckHandler(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, CardData cardData) {
            if (playerEnemyMonoBehaviour.IsPlayers != IsPlayers)
                return;
            StartCoroutine(AddToEndOfDeck(cardData));
        }
        private void OnDrawCardHandler(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, IHasCost iHasCost, int amount) {
            if (playerEnemyMonoBehaviour.IsPlayers != IsPlayers)
                return;
            StartCoroutine(Draw(amount, iHasCost));
        }

        private void OnCardPlayedHandler(Card card, ICardPlayTarget target,bool isSummoned = false) {
            
            var cost = card.GetCost();
            if (isSummoned) 
                cost = 0;
            
            var costObject = new HasCost(cost);
            
            if (card.IsPlayers != IsPlayers)
                return;
            if (!IsYourTurn || isSummoned)
                return;
            if (!manaManager.CanUseMana(costObject, true) )
                return;


            bool wasPlayed = false;
            switch (card) {
                case Minion minion:
                    wasPlayed = PlayMinion(minion, target);
                    break;
                case Spell spell:
                    wasPlayed = PlaySpell(spell, target);
                    break;
                default:
                    Debug.LogError("Card type not valid");
                    break;
            }

            if (!wasPlayed)
                return;

            manaManager.UseMana(costObject);
            hand.RemoveCard(card,!isSummoned);
            card.cardDragging.droppedOnSlot = true;
            StartCoroutine(card.Play());
            if(!isSummoned)
                hand.UpdateCardPositions();
        }

        private void OnForceAddToHandHandler(PlayerEnemyMonoBehaviour playerEnemyMonoBehaviour, CardData cardData, int amount) {
            if (playerEnemyMonoBehaviour.IsPlayers != IsPlayers)
                return;
            StartCoroutine(ForceAddCardsToHandCoroutine(cardData, amount));
        }
        
        private bool PlayMinion(Minion minion, ICardPlayTarget target) {
            switch (target) {
                case CardSpot cardSpot:
                    MoveCardToCardSpot(minion, cardSpot);
                    break;
                default:
                    WrongCardTargetCombo();
                    return false;
            }

            return true;
        }


        //TODO FIX
        private bool PlaySpell(Spell spell, ICardPlayTarget target) {
            return true;
        }

        private void WrongCardTargetCombo() {
            Debug.LogError("This card cannot be played at given cardSpot");
        }

        public IEnumerator PlayCardCoroutine(Card card, ICardPlayTarget target, float time) { //time defined in EnemyAi
            OnCardPlayedHandler(card, target);
            yield return new WaitForSeconds(time);
        }


        private void MoveCardToCardSpot(Card card, CardSpot cardSpot) {
            //TODO change all of these to functions inside card 
            if (!IsPlayers)
                StartCoroutine(
                    card.GetComponent<CardAnimation>()
                        .PlayToCardSpotAnimation(cardSpot));

            card.GetComponent<CardDisplay>().ChangeCardVisible(true);
            if (IsPlayers)
                card.GetComponent<RectTransform>().position =
                    cardSpot.GetComponent<RectTransform>().position;
            else {
                StartCoroutine(
                    card.GetComponent<CardAnimation>()
                        .PlayToCardSpotAnimation(cardSpot));
            }

            card.AssignCardSpot(cardSpot);
            card.transform.SetParent(cardSpot.transform);

            //TODO change so that it works not only from hand
            cardSpot.card = card;
            card.AssignCardSpot(cardSpot);
        }

        public IEnumerator Draw(int amount, IHasCost iHasCost) {
            yield return StartCoroutine(Draw(amount, iHasCost.GetCost()));
        }

        public IEnumerator Draw(int amount, int cost = 0) {
            if (amount <= 0) {
                Debug.LogError("Player just tried to draw 0 or less cards");
                yield break;
            }

            if (cost > 0)
                if (!manaManager.TryUseMana(cost))
                    yield break;

            var cardsToDraw = new List<Card>();
            for (int i = 0; i < amount; i++) {
                if (!deck.HasCards) {
                    //TODO ADD FEEDBACK CURRENTLY IS JUST DEBUG LOG
                    deck.NoMoreCards();
                    break;
                }

                cardsToDraw.Add(deck.PopTopCard());
            }

            yield return hand.DrawManyCoroutine(cardsToDraw);
        }

        
        
        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator StartOfTurn() {
            if (IsPlayers)
                StartCoroutine(TurnChangeSoundEffect());
            yield return Draw(1);

            manaManager.RefreshMana();
            var cards = boardSide.GetNoNullCards();
            foreach (var card in cards) {
                card.DoEffect(EffectTrigger.OnStartTurn);
            }
        }

        public IEnumerator TurnChangeSoundEffect() {
            yield return new WaitForSeconds(0.4f);
            var clipName = "TurnStart";
            var x = AudioCollection.Instance.GetClip(clipName);
            AudioManager.Instance.Play(x);
        }

        public void DrawACard(int cost = 1) {
            StartCoroutine(Draw(1, cost));
        }


        public IEnumerator EndOfTurn() {
            yield return StartCoroutine(BoardManager.Instance.Attack(IsPlayers));
            foreach (var card in boardSide.GetNoNullCards()) {
                card.DoEffect(EffectTrigger.OnEndTurn);
            }

            yield return null;
        }
    }
}