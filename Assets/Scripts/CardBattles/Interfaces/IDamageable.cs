
using UnityEngine;

namespace CardBattles.Interfaces {
    public interface IDamageable {
        public void TakeDamage(int amount, bool isInstaKill = false);
        public void Heal(int amount);
        public void Die();
        public Transform GetTransform();
        public void BuffHp(int amount);
    }
}