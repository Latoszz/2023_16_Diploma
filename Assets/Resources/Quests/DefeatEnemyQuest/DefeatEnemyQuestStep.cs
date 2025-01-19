using System;
using System.Collections.Generic;
using System.Linq;
using EnemyScripts;
using QuestSystem;
using UnityEngine;

public class DefeatEnemyQuestStep : QuestStep {
    [SerializeField] private string enemyID;
    private Enemy enemy;
    private bool defeated;

    private void Awake() {
        IEnumerable<Enemy> objects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<Enemy>();
        List<Enemy> allEnemies = new List<Enemy>(objects);
        foreach (var e in allEnemies) {
            if (e.GetID() == enemyID)
                enemy = e;
        }
    }
    private void Update() {
        if (enemy.GetState() != EnemyState.Defeated) return;
        defeated = true;
        UpdateState();
        FinishQuestStep();
    }

    private void UpdateState() {
        string state = defeated.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        defeated = Convert.ToBoolean(state);
        UpdateState();
    }
    
    protected override void InitializeQuestStepState() {
        UpdateState();
    }
}
