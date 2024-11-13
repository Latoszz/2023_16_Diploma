using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBattles.Character;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using UnityEngine;

namespace CardBattles.Managers {
    public static class EffectManager {
        private static Dictionary<EffectName, EffectVisualsManager.EffectAnimationDelegate> Visual =>
            EffectVisualsManager.Instance.visual;

        public delegate IEnumerator EffectDelegate(List<GameObject> targets, int value);

        public static Dictionary<EffectName, EffectDelegate>
            effectDictionary =
                new() {
                    { EffectName.Heal, Heal },
                    { EffectName.DealDamage, DealDamage },
                    { EffectName.ChangeAttack, ChangeAttack },
                    { EffectName.BuffHp, BuffHp },
                    { EffectName.DrawACard, DrawACard },
                    { EffectName.DealOrHeal, DealOrHeal }
                };

        private static IEnumerator DealOrHeal(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    if (Random.value > 0.5f) {
                        ((IDamageable)component).Heal(value);
                        yield return Visual[EffectName.Heal](component);
                    }
                    else {
                        ((IDamageable)component).TakeDamage(value);
                        yield return Visual[EffectName.DealDamage](component);
                    }
                }
            }
        }

        private static IEnumerator Heal(List<GameObject> targets, int heal) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).Heal(heal);
                    yield return Visual[EffectName.Heal](component);
                }
            }
        }


        private static IEnumerator DealDamage(List<GameObject> targets, int damage) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).TakeDamage(damage);
                    yield return Visual[EffectName.DealDamage](component);
                }
            }
        }

        private static IEnumerator ChangeAttack(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IAttacker), out var component)) {
                    ((IAttacker)component).ChangeAttackBy(value);
                    yield return Visual[EffectName.DealDamage](component);
                }
            }
        }

        private static IEnumerator BuffHp(List<GameObject> targets, int value) {
            foreach (var target in targets) {
                if (target.TryGetComponent(typeof(IDamageable), out var component)) {
                    ((IDamageable)component).BuffHp(value);
                    yield return Visual[EffectName.BuffHp](component);
                }
            }
        }

        //Expects a hero
        private static IEnumerator DrawACard(List<GameObject> targets, int value) {
            var x = targets.First();
            if (!x.TryGetComponent(typeof(PlayerEnemyMonoBehaviour), out var playerEnemyMonoBehaviour))
                yield break;
            CharacterManager.DrawACard((PlayerEnemyMonoBehaviour)playerEnemyMonoBehaviour,0);
            yield return null;
        }
    }
}