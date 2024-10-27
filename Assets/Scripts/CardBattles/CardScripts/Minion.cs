using System;
using System.Collections;
using System.Linq;
using Audio;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Managers;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CardBattles.CardScripts {
    public class Minion : Card, IAttacker {
        [SerializeField] private float dyingDuration = 0.5f;
        [SerializeField] public UnityEvent<int, int, int> dataChanged;

        [Space(20), Header("Minion")] [HorizontalLine(1f)] [BoxGroup("Data")] [SerializeField]
        private int attack;

        private int Attack {
            get => attack;
            set {
                attack = math.max(value, 0);
                dataChanged?.Invoke(attack, currentHealth, maxHealth);
            }
        }

        [SerializeField] [BoxGroup("Data")]
        private int maxHealth;

        private int MaxHealth {
            get => maxHealth;
            set {
                maxHealth = value;
                dataChanged?.Invoke(attack, currentHealth, maxHealth);
                if (CurrentHealth > maxHealth)
                    CurrentHealth = value;
            }
        }

        [BoxGroup("Data")] private int currentHealth;

        private int CurrentHealth {
            get => currentHealth;
            set {
                currentHealth = value > MaxHealth ? MaxHealth : value;
                dataChanged?.Invoke(attack, currentHealth, maxHealth);
                if (currentHealth <= 0) {
                    Die();
                }
            }
        }

        public override void Initialize(CardData cardData, bool isPlayersCard) {
            base.Initialize(cardData, isPlayersCard);

            if (cardData is not MinionData minionData) {
                Debug.LogError("Invalid data type passed to Minion.Initialize");
            }
            else {
                Attack = minionData.attack;
                MaxHealth = minionData.maxHealth;
                CurrentHealth = MaxHealth;
                cardDisplay.SetCardDisplayData(minionData);
            }

            dataChanged.AddListener(cardDisplay.UpdateData);
        }


        public Action<Vector3, IDamageable> action;
        public int GetAttack() => Attack;
        public void ChangeAttackBy(int amount) => Attack += amount;

        [SerializeField] private string takeDamageSound = "Damage.Card";
        public void TakeDamage(int amount, bool isInstaKill = false) {
                
                
            amount = amount > 0 ? amount : 0;
            CurrentHealth -= amount;
            if (properties == null) {
                Debug.LogWarning("Properties list is not initialized in Minion.");
            }
            if (!(properties.Contains(AdditionalProperty.ImmuneToPoison)) && isInstaKill) {
                
                Die();
            }

            var x =AudioCollection.Instance.GetClip(takeDamageSound);
            AudioManager.Instance.PlayWithVariation(x);
        }
        

        public void Heal(int amount) {
            amount = amount > 0 ? amount : 0;
            CurrentHealth += amount;
        }

        public void Die() {
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine() {
            yield return StartCoroutine(cardAnimation.Die());
            
            yield return new WaitForSeconds(dyingDuration);
            Destroy(gameObject);
        }

        public Transform GetTransform() {
            return transform;
        }

        public void AttackTarget(IDamageable target) {
            if(properties.Contains(AdditionalProperty.Chance50ToNotAttack))
                if (Random.value < 0.5f) {
                    transform.DOShakePosition(0.5f,20f,20);
                    return;
                }

            StartCoroutine(
                cardAnimation.AttackAnimation(
                    this, target));
        }

        public IEnumerator ChangeSortingOrderTemporarily(int num) {
            canvas.sortingOrder += num;
            yield return new WaitForSeconds(2f);
            canvas.sortingOrder -= num;
        }
        public void BuffHp(int amount) {
            Debug.Log($"{name} buffed by {amount}");
            MaxHealth += amount;
            Heal(amount);
        }
        private void OnDestroy() {
            if (isPlacedAt is not null) {
                if (isPlacedAt.card is not null) {
                    isPlacedAt.card = null;
                }

                isPlacedAt = null;
            }
        }
    }
}