using System;
using EnemyScripts;

namespace Events {
    public class EnemyEvents {
        public event Action<Enemy, EnemyState> OnEnemyStateChange;
        
        public void EnemyChangeState(Enemy enemy, EnemyState state) {
            OnEnemyStateChange?.Invoke(enemy, state);
        }
        
    }
}