using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Character;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardBattles.Managers {
    public static class EffectManager {
        public delegate IEnumerator EffectDelegate(List<GameObject> targets, int value);

        public static Dictionary<EffectName, EffectDelegate>
            effectDictionary =
                new() {
                    { EffectName.Heal, Heal },
                    { EffectName.DealDamage, DealDamage },
                    { EffectName.ChangeAttack, ChangeAttack },
                    { EffectName.BuffHp, BuffHp },
                    { EffectName.DrawACard, DrawACard },
                    { EffectName.DealOrHeal, DealOrHeal },
                    { EffectName.HealAndBuff, HealAndBuff },
                    { EffectName.BuffAttackAndHp, BuffAttackAndHp },
                    { EffectName.SummonStrawmen, SummonStrawmen },
                    { EffectName.SummonFoxes, SummonFoxes },
                    { EffectName.ReplaceWithSapling, ReplaceWithSapling },
                    { EffectName.AddToHandAnts, AddToHandAnts },
                    { EffectName.SummonSkeleton, SummonSkeleton },
                    { EffectName.DealDamageToOneRandom, DealDamageToOneRandom},
                    { EffectName.SummonCrewmate, SummonCrewmate}
                };


        private static IEnumerator DoVisuals(EffectName effect, Component target) {
            yield return EffectVisualsManager.Instance.ExecuteVisualEffect(effect, target);
        }

        private static IEnumerator Heal(List<GameObject> targets, int heal) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).Heal(heal);
                    yield return DoVisuals(EffectName.Heal, component);
                }
            }
        }


        private static IEnumerator DealDamage(List<GameObject> targets, int damage) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).TakeDamage(damage);
                    yield return DoVisuals(EffectName.DealDamage, component);
                }
            }

            yield return null;
        }


        private static IEnumerator ChangeAttack(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IAttacker), out var component)) {
                    ((IAttacker)component).ChangeAttackBy(value);
                    yield return DoVisuals(EffectName.ChangeAttack, component);
                }
            }
        }

        private static IEnumerator BuffHp(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).BuffHp(value);
                    yield return DoVisuals(EffectName.BuffHp, component);
                }
            }
        }


        //Expects a hero

        private static IEnumerator DrawACard(List<GameObject> targets, int value) {
            var x = targets.First();
            if (!x.TryGetComponent(typeof(PlayerEnemyMonoBehaviour), out var playerEnemyMonoBehaviour))
                yield break;
            CharacterManager.DrawACard((PlayerEnemyMonoBehaviour)playerEnemyMonoBehaviour, 0);
        }

        private static IEnumerator DealOrHeal(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    if (Random.value > 0.5f) {
                        ((IDamageable)component).Heal(value);
                        yield return DoVisuals(EffectName.Heal, component);
                    }
                    else {
                        ((IDamageable)component).TakeDamage(value);
                        yield return DoVisuals(EffectName.DealDamage, component);
                    }
                }
            }
        }


        private static IEnumerator HealAndBuff(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).Heal(value);
                }
            }

            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IAttacker), out var component)) {
                    ((IAttacker)component).ChangeAttackBy(value);
                    yield return DoVisuals(EffectName.HealAndBuff, component);
                }
            }
        }

        private static IEnumerator BuffAttackAndHp(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).BuffHp(value);
                }
            }

            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IAttacker), out var component)) {
                    ((IAttacker)component).ChangeAttackBy(value);
                    yield return DoVisuals(EffectName.BuffAttackAndHp, component);
                }
            }
        }

        private static IEnumerator AddToHandAnts(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("ToSummon", "Ant");
            yield return AddCardToHand(targets, value, card);
        }

        private static IEnumerator AddCardToHand(List<GameObject> targets, int value, CardData card) {
            if (targets.Count < 1)
                yield break;
            if (card == null)
                yield break;
            if (value <= 0)
                yield break;

            if (!targets[0].TryGetComponent(typeof(PlayerEnemyMonoBehaviour), out var playerEnemyMonoBehaviour))
                yield break;
            CharacterManager.AddCardsToHand((PlayerEnemyMonoBehaviour)playerEnemyMonoBehaviour, card, value);
        }


        //EXPECTS CARDSPOTS AS TARGETS
        //Value determines amount

        private static IEnumerator SummonFoxes(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("ToSummon", "Fox");
            yield return SummonUnits(targets, value, card);
        }

        private static IEnumerator SummonSkeleton(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("ToSummon", "Skeleton");
            yield return SummonUnits(targets, value, card);
        }
        private static IEnumerator SummonCrewmate(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("ToSummon", "Undecided Crewman");
            yield return SummonUnits(targets, value, card);
        }

        private static IEnumerator SummonStrawmen(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("Tutorial", "StrawMan");
            yield return SummonUnits(targets, value, card);
        }

        private static IEnumerator SummonUnits(List<GameObject> targets, int value, CardData unit) {
            if (unit == null)
                yield break;
            var i = 0;
            foreach (var target in targets) {
                if (i >= value)
                    break;
                i++;
                if (!target.TryGetComponent(typeof(CardSpot), out var component))
                    continue;

                var card = CardManager.Instance.CreateCard(unit, (CardSpot)component);
                CharacterManager.SummonACard(card, (CardSpot)component);
            }
        }

        private static IEnumerator ReplaceWithSapling(List<GameObject> targets, int value) {
            var card = CardManager.Instance.LoadCardData("Nature's Guardian", "Sapling");
            yield return ReplaceWithMinion(targets, value, card);
        }

        //expects itself as target
        private static IEnumerator ReplaceWithMinion(List<GameObject> targets, int value, CardData unit) {
            var target = targets.First();
            if (!target.TryGetComponent(typeof(Minion), out var component))
                yield break;
            var minion = (Minion)component;
            var cardSpot = minion.isPlacedAt;
            GameObject[] minionAsArray = { minion.gameObject };
            yield return Kill(minionAsArray.ToList(), 0);
            yield return new WaitForSecondsRealtime(2f);

            GameObject[] cardSpotAsArray = { cardSpot!.gameObject };
            yield return SummonUnits(cardSpotAsArray.ToList(), 1, unit);
        }


        private static IEnumerator Kill(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).Die();
                    //yield return DoVisuals(EffectName.DealDamage, component);
                }
            }

            yield return null;
        }

        private static IEnumerator DealDamageToOneRandom(List<GameObject> targets, int value) {
            if (targets.Count < 1)
                yield break;
            
            var randomIndex = (int)(Random.value * targets.Count);
            var target = targets[randomIndex];
            var targetAsList = new List<GameObject>{target};
            yield return DealDamage(targetAsList, value);
        }

    }
}

