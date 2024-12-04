using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts;
using CardBattles.CardScripts.temp;
using CardBattles.Enums;
using CardBattles.Managers;
using CardBattles.Managers.GameSettings;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Math = System.Math;
using Random = System.Random;

namespace CardBattles.Character {
    public class EnemyAi : MonoBehaviour {
        private CharacterManager character;
        private Random random;

        [SerializeField] private float waitBetweenPlayingCards;

        [SerializeField] private EnemyBrainDictionary baseWeights;

        private List<Card> CardsInHand => character.hand.Cards.ToList();
        private List<Minion> MinionsInHand => CardsInHand.OfType<Minion>().ToList();
        private List<Spell> SpellsInHand => CardsInHand.OfType<Spell>().ToList();

        private Queue<EnemyAiAction> predefinedActions;

        [ShowNativeProperty]
        private int MinionCount {
            get {
                if (Application.isPlaying) {
                    return MinionsInHand.Count;
                }

                return 0;
            }
        }

        [ShowNativeProperty]
        private int SpellCount {
            get {
                if (Application.isPlaying) {
                    return SpellsInHand.Count;
                }

                return 0;
            }
        }
        

        
        [Foldout("Debug"), SerializeField]
        private bool addDelayAfterThinking; 

        [Foldout("Debug"), SerializeField]
        private EnemyBrainDictionary weights;

        
        private void Awake() {
            random = new Random();
            character = GetComponent<CharacterManager>();
            CopyWeightValues();
            if (GameStats.isTutorial) {
                predefinedActions = new Queue<EnemyAiAction>(GameStats.Config.tutorialData.enemyActions);
            }
        }

        private void CopyWeightValues() {
            weights = new EnemyBrainDictionary();
            foreach (var key in baseWeights.Keys) {
                weights.Add(key, baseWeights[key]);
            }
        }

        private IEnumerator PlayAMinion() {
            var hand = MinionsInHand;
            var cardSpots = character.boardSide.GetEmptyCardSpots();


            if (!hand.Any() || !cardSpots.Any()) {
                Debug.LogError("this shouldn't happen");
                yield break;
            }
            var minionIndex = GameStats.isTutorial ? 0 : random.Next(hand.Count);

            var card = hand[minionIndex];
            while (card is not Minion) {
                Debug.Log("should not happen");
                card = hand[random.Next(hand.Count)];
            }

            var cardSpot = cardSpots[random.Next(cardSpots.Count)];

            yield return character.PlayCardCoroutine(card, cardSpot, waitBetweenPlayingCards);
        }

        private IEnumerator PlayASpell() {
            var hand = SpellsInHand;


            if (!hand.Any()) {
                Debug.LogError("this shouldn't happen");
                yield break;
            }

            var spellIndex = GameStats.isTutorial ? 0 : random.Next(hand.Count);
            var card = hand[spellIndex];
            while (card is not Spell) {
                Debug.Log("should not happen");
                card = hand[random.Next(hand.Count)];
            }


            yield return character.PlayCardCoroutine(card, SpellPlayTarget.Instance, waitBetweenPlayingCards);
        }

        public IEnumerator PlayTurn(bool repeat = true) {
            CopyWeightValues();

            if (CantDoNextAction()) {
                yield break;
            }

            var nextAction = ChooseActionToDo();

            #if UNITY_EDITOR
            if(addDelayAfterThinking)
                yield return new WaitForSeconds(1f);
            #endif
            
            switch (nextAction) {
                case EnemyAiAction.Draw:
                    yield return character.Draw(1, 1);
                    break;
                case EnemyAiAction.PlayMinion:
                    yield return PlayAMinion();
                    break;
                case EnemyAiAction.PlaySpell:
                    yield return PlayASpell();
                    yield return new WaitForSeconds(0.3f);
                    break;
                case EnemyAiAction.Pass:
                    yield break;
                default:
                    Debug.LogError("Enemy tried to do forbidden action");
                    yield break;
            }

            if (repeat)
                yield return StartCoroutine(PlayTurn());
            else {
                yield break;
            }
        }

        private bool CantDoNextAction() {
            return TurnManager.Instance.gameHasEnded || !character.IsYourTurn;
        }
        
        private EnemyAiAction ChooseActionToDo() {
            if (GameStats.isTutorial && predefinedActions.Any()) {
                return predefinedActions.Dequeue();
            }
            if(!ModifyProbabilities())
                return EnemyAiAction.Pass;


            float totalWeight = 0;
            foreach (var item in weights) {
                totalWeight += item.Value;
            }

            
            
            if (totalWeight - weights[EnemyAiAction.Pass] <= 0) {
                return EnemyAiAction.Pass;
            }
            
            double randomValue = random.NextDouble() * totalWeight;


            foreach (var item in weights.Keys) {
                randomValue -= weights[item];
                if (randomValue <= 0) {
                    return item;
                }
            }

            Debug.Log("This shouldnt happen, check enemyAi");
            return EnemyAiAction.Pass;
        }


        //TODO MAGIC NUMBERS
        private bool ModifyProbabilities() {
            if (NoMana())
                return false;
            
            if (SpellsArePlayable())
                ModifySpellPlayWeight();

            if (MinionArePlayable())
                ModifyMinionPlayWeight();

            if (CanDrawCards())
                ModifyCardDrawWeights();
            return true;
        }

        private bool NoMana() {
            return character.manaManager.CurrentMana <= 0;
        }


        //ModifyWeights
        private void ModifyMinionPlayWeight() {
            var multiplyBy = 1f;
            multiplyBy *= (float)Math.Sqrt(MinionsInHand.Count);
            multiplyBy *= 2 * (character.boardSide.GetEmptyCardSpots().Count / 4f);

            weights[EnemyAiAction.PlayMinion] *= multiplyBy;
        }

        private void ModifySpellPlayWeight() {
            var multiplyBy = 1f;
            multiplyBy *= (float)Math.Sqrt(SpellsInHand.Count);

            weights[EnemyAiAction.PlaySpell] *= multiplyBy;
        }

        private void ModifyCardDrawWeights() {
            var multiplyBy = 1f;
            multiplyBy *= 2f / (CardsInHand.Count + 0.1f); // + 0.1f to avoid divide by 0

            weights[EnemyAiAction.Draw] *= multiplyBy;
        }


        //PlayableChecks
        //Spell
        private bool SpellsArePlayable() {
            if (NoSpellInHand()) {
                weights[EnemyAiAction.PlaySpell] = 0f;
                return false;
            }

            return true;
        }

        private bool NoSpellInHand() {
            return SpellsInHand.Count <= 0;
        }


        //Minion
        private bool MinionArePlayable() {
            if (NoMinionsInHand() || NoEmptyCardSpots()) {
                weights[EnemyAiAction.PlayMinion] = 0f;
                return false;
            }

            return true;
        }

        private bool NoMinionsInHand() {
            return MinionsInHand.Count <= 0;
        }

        private bool NoEmptyCardSpots() {
            return !character.boardSide.GetEmptyCardSpots().Any();
        }

        //Draw
        private bool CanDrawCards() {
            if (DeckIsEmpty()) {
                weights[EnemyAiAction.Draw] = 0f;
                return false;
            }

            return true;
        }

        private bool DeckIsEmpty() {
            return !character.deck.HasCards;
        }
    }
}