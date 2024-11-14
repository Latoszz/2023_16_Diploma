using System;
using CardBattles.CardGamesManager;
using Events;
using SaveSystem;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private string enemyID;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        enemyID = Guid.NewGuid().ToString();
    }

    [SerializeField] private BattleData battleData;
    [SerializeField] private Material undefeatedMaterial;
    [SerializeField] private Material defeatedMaterial;

    private EnemyState state = EnemyState.Locked;

    private void Start() {
        if (state == EnemyState.Defeated) {
            this.gameObject.SetActive(false);
        }
    }
    
    public void ChangeState(EnemyState state) {
        this.state = state;
        if (state == EnemyState.Defeated) {
            SaveManager.Instance.ChangeEnemyData(enemyID, state);
        }
    }
    
    public EnemyState GetState() {
        return state;
    }

    public BattleData GetBattleData() {
        return battleData;
    }

    public string GetID() {
        return enemyID;
    }
}
