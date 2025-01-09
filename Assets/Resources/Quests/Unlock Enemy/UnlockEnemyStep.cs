using System.Collections.Generic;
using System.Linq;
using EnemyScripts;
using QuestSystem;
using UnityEngine;

public class UnlockEnemyStep : QuestStep {
    [SerializeField] private string enemyToUnlockID;
    
    void Start() {
        IEnumerable<Enemy> objects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<Enemy>();
        List<Enemy> enemies = new List<Enemy>(objects);
        foreach (var enemy in enemies) {
            if (enemy.GetID() != enemyToUnlockID) continue;
            enemy.gameObject.SetActive(true);
            enemy.ChangeState(EnemyState.Undefeated);
            FinishQuestStep();
            return;
        }
    }


    protected override void SetQuestStepState(string state) {
        
    }

    protected override void InitializeQuestStepState() {
        
    }
}
