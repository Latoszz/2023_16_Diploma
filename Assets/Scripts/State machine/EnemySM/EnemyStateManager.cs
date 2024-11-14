using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    private EnemySM currentEnemy;

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

    public void ChangeEnemyState(EnemyState state) {
        currentEnemy.ChangeState(state);
    }
}