using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    private Enemy currentEnemy;

    public static EnemyStateManager Instance;

    private void Awake() {
        if (Instance is not null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void SetCurrentEnemy(Enemy enemy) {
        currentEnemy = enemy;
    }

    public Enemy GetCurrentEnemy() {
        return currentEnemy;
    }

    public void ChangeEnemyState(EnemyState state) {
        currentEnemy.ChangeState(state);
    }
}