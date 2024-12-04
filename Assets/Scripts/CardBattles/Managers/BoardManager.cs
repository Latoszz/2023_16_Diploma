using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts;
using CardBattles.CardScripts.temp;
using CardBattles.Character;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using UnityEngine;

namespace CardBattles.Managers {
    public class BoardManager : MonoBehaviour {
        [SerializeField] private float betweenAttacksTime;

        public static BoardManager Instance { get; private set; }

        [SerializeField] private CharacterManager playerCharacter;
        [SerializeField] private CharacterManager enemyCharacter;
        private BoardSide player;
        private BoardSide enemy;


        private CharacterManager PlayingCharacter(bool isPlayers) => isPlayers ? playerCharacter : enemyCharacter;
        private CharacterManager WaitingCharacter(bool isPlayers) => isPlayers ? enemyCharacter : playerCharacter;

        private BoardSide Playing(bool isPlayers) => isPlayers ? player : enemy;
        private BoardSide Waiting(bool isPlayers) => isPlayers ? enemy : player;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }

            player = playerCharacter.boardSide;
            enemy = enemyCharacter.boardSide;
        }


        public IEnumerator Attack(bool isPlayers) {
            var attacker = Playing(isPlayers);
            var target = Waiting(isPlayers);

            var coroutine = StartCoroutine(
                AttackCourutine(
                    attacker.GetIAttackers(),
                    target.GetIDamageables()));
            yield return coroutine;
        }

        private IEnumerator AttackCourutine(List<IAttacker> attackers, List<IDamageable> targets) {
            if (attackers.Count != 4)
                Debug.LogError($"{attackers.Count}  attack >:(");
            if (targets.Count != 4)
                Debug.LogError($"{targets.Count}  target >:(");


            for (int i = 0; i < 4; i++) {
                if (attackers[i] is null)
                    continue;
                if (attackers[i].GetAttack() <= 0)
                    continue;
                if (attackers[i] is Minion minion) {
                    StartCoroutine(minion.ChangeSortingOrderTemporarily(10 + i));
                }

                attackers[i].AttackTarget(targets[i]);
                yield return new WaitForSeconds(betweenAttacksTime);
            }
        }

        public delegate List<GameObject> TargetsDelegate(TargetType targetType, Card card);


        public List<GameObject> GetTargets(TargetType targetType, Card card) {
            bool isPlayers = card.IsPlayers;
            List<GameObject> targets = new List<GameObject>();

            switch (targetType) {
                case TargetType.AllMinions:
                    targets.AddRange(player.GetNoNullCardsObjects());
                    targets.AddRange(enemy.GetNoNullCardsObjects());
                    break;
                case TargetType.EnemyMinions:
                    targets.AddRange(Waiting(isPlayers).GetNoNullCardsObjects());
                    break;
                case TargetType.YourMinions:
                    targets.AddRange(Playing(isPlayers).GetNoNullCardsObjects());
                    break;
                case TargetType.OpposingMinion:
                    targets.AddRange(GetOpposingCard(card));
                    break;
                case TargetType.ThisMinion:
                    targets.Add(card.gameObject);
                    break;
                case TargetType.AdjacentMinions:
                    targets.AddRange(Playing(isPlayers).GetAdjecentCards(card));
                    break;
                case TargetType.BothHeroes:
                    targets.Add(player.hero.gameObject);
                    targets.Add(enemy.hero.gameObject);
                    break;
                case TargetType.YourHero:
                    targets.Add(Playing(isPlayers).hero.gameObject);
                    break;
                case TargetType.OpposingHero:
                    targets.Add(Waiting(isPlayers).hero.gameObject);
                    break;
                case TargetType.CardSetAll:
                    targets.AddRange(PlayingCharacter(isPlayers).deck.GetCardFromSameCardSet(card));
                    break;
                case TargetType.CardSetNotThis:
                    targets.AddRange(PlayingCharacter(isPlayers).deck.GetOtherCardFromSameCardSet(card));
                    break;
                case TargetType.Allies:
                    targets.AddRange(Playing(isPlayers).GetNoNullCardsObjects());
                    targets.Add(Playing(isPlayers).hero.gameObject);
                    break;
                case TargetType.Enemies:
                    targets.AddRange(Waiting(isPlayers).GetNoNullCardsObjects());
                    targets.Add(Waiting(isPlayers).hero.gameObject);
                    break;
                case TargetType.NoneButGetWhoseIsIt:
                    targets.Add(card.gameObject);
                    break;
                case TargetType.None:
                    break;
                case TargetType.YourCardSpots:
                    foreach (var cardSpot in Playing(isPlayers).GetEmptyCardSpots()) {
                        targets.Add(cardSpot.gameObject);
                    }

                    break;
                case TargetType.EnemyCardSpots:
                    foreach (var cardSpot in Waiting(isPlayers).GetEmptyCardSpots()) {
                        targets.Add(cardSpot.gameObject);
                    }

                    break;
                default:
                    break;
            }

            return targets;
        }

        private IEnumerable<GameObject> GetOpposingCard(Card card) {
            if (card is not Minion) return new List<GameObject>();


            var pc = Playing(card.IsPlayers);
            var index = pc.ContainsCardAtIndex(card);

            if (index == -1) return new List<GameObject>();

            var enemyCard = Waiting(card.IsPlayers).cardSpots[index].card;

            if (enemyCard is null) return new List<GameObject>();

            return new List<GameObject> { enemyCard.gameObject };
        }
    }
}