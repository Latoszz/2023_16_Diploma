using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    private List<EnemySM> enemyList = new List<EnemySM>();
    private EnemySM currentEnemy;
    private GameObject[] allEnemies;

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

    public void SetCurrentEnemy(EnemySM enemy) {
        currentEnemy = enemy;
    }

    public Enemy GetCurrentEnemy() {
        return currentEnemy.GetEnemy();
    }
}