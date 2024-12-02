using System;
using CardBattles.Interfaces;
using CardBattles.Managers.GameSettings;
using UnityEngine;

namespace CardBattles.Character.Hero {
    public class Hero : MonoBehaviour,IDamageable {

        public Action<bool> death;
        
        [SerializeField]
        private int maxHealth = 30;

        public int MaxHealth {
            get => maxHealth;
            set {
                maxHealth = value;
                if(CurrentHealth>maxHealth)
                    CurrentHealth = value;
            }
        }

        public Action<int> currentHealthAction;
        public Action takeDamageAction;


        private void Start() {
            if (GameStats.Config.overrideHeroMaxHp)
                MaxHealth = GameStats.Config.overrideHeroMaxHpValue;
        }


        [SerializeField]
        
        public int currentHealth;

        private int CurrentHealth {
            get => currentHealth;
            set {
                currentHealth = value > MaxHealth ? MaxHealth : value;
                
                currentHealthAction.Invoke(currentHealth);
                if (currentHealth <= 0) {
                    Die();
                }
            }
        }
        public bool HasFullHp => CurrentHealth == MaxHealth;

        
        private bool isPlayers;

        private void Awake() {
            isPlayers = CompareTag("Player");
        }

        public void TakeDamage(int amount, bool isPoisonous) {
            CurrentHealth -= amount;
            takeDamageAction.Invoke();
        }

        public void Heal(int amount) {
            CurrentHealth += amount;
        }

        public void Die() {
            death.Invoke(isPlayers);
            
        }

        public Transform GetTransform() {
            return transform;
        }

        public void BuffHp(int amount) {
            MaxHealth += amount;
        }
    }
}