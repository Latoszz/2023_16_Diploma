using System;
using System.Collections;
using System.Linq;
using Audio;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Managers;
using CardBattles.Managers.GameSettings;
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

        public int baseAttack;
        public int baseMaxHealth;
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
                baseAttack = minionData.attack;
                MaxHealth = minionData.maxHealth;
                baseMaxHealth = minionData.maxHealth;
                CurrentHealth = MaxHealth;
                cardDisplay.SetCardDisplayData(minionData);
                if(GameStats.Config.cardsExtraSleep)
                    Properties.Add(AdditionalProperty.Just_Played);

            }

            dataChanged.AddListener(cardDisplay.UpdateData);
        }


        public Action<Vector3, IDamageable> action;
        public int GetAttack() => Attack;
        public void ChangeAttackBy(int amount) => Attack += amount;

        [SerializeField] private UnityEvent poisonTrigger; 
        [SerializeField] private UnityEvent immuneToPoisonTrigger; 
        [SerializeField] private string takeDamageSound = "Damage.Card";
        public void TakeDamage(int amount, bool isPoisonous = false) {

            if (Properties.Contains(AdditionalProperty.Durable))
                amount = (amount + 1) / 2; //divide/2 round up

            if (Properties.Contains(AdditionalProperty.Spiky)) 
                TriggerSpiky();
            

            amount = amount > 0 ? amount : 0;
            CurrentHealth -= amount;
            
            PoisonCheck(isPoisonous);
            
            var x =AudioCollection.Instance.GetClip(takeDamageSound);
            AudioManager.Instance.PlayWithVariation(x);
        }
        
        private void TriggerSpiky() {
            var targets = BoardManager.Instance.GetTargets(TargetType.OpposingMinion, this);
            if(!targets.Any())
                return;
            PersistentEffectManager.Instance.DoEffect(targets,EffectName.DealDamage,1);
        }
        
        private void PoisonCheck(bool isPoisonous) {
            if (!isPoisonous) return;
            
            poisonTrigger?.Invoke();
            if (Properties.Contains(AdditionalProperty.Immune_To_Poison)) {
                immuneToPoisonTrigger?.Invoke();
            }
            else
                Die();
        }

        
        
        [SerializeField] private UnityEvent unhealableProc;
        public void Heal(int amount) {
            if (Properties.Contains(AdditionalProperty.Unhealable)) {
                unhealableProc?.Invoke();
                return;
            }
            amount = amount > 0 ? amount : 0;
            CurrentHealth += amount;
        }

        public void Die() {
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine() {
            yield return StartCoroutine(DoEffect(EffectTrigger.OnDeath));
            
            yield return StartCoroutine(cardAnimation.Die());
            
            yield return new WaitForSeconds(dyingDuration);
            Destroy(gameObject);
        }

        public Transform GetTransform() {
            return transform;
        }

        [SerializeField] private UnityEvent stopsSleeping;
        public void AttackTarget(IDamageable target) {

            if (Properties.Contains(AdditionalProperty.Just_Played)) {
                Properties.Remove(AdditionalProperty.Just_Played);
                transform.DOShakePosition(0.5f,20f,20);
                return;
            }
            
            if (Properties.Contains(AdditionalProperty.Sleepy)) {
                Properties.Remove(AdditionalProperty.Sleepy);
                transform.DOShakePosition(0.5f,20f,20);
                stopsSleeping?.Invoke();
                return;
            }
            
            if(Attack <=0 )
                return;
            
            if(Properties.Contains(AdditionalProperty.Lazy))
                if (Random.value < 0.5f) {
                    transform.DOShakePosition(0.5f,20f,20);
                    return;
                }

            StartCoroutine(
                cardAnimation.AttackAnimation(
                    this, target));
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