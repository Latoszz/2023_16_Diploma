using System;
using System.Collections.Generic;
using CardBattles.CardGamesManager;
using Interaction.Objects;
using SaveSystem;
using UnityEngine;

namespace EnemyScripts {
    public class Enemy : MonoBehaviour {
        [SerializeField] private string enemyID;
        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            enemyID = Guid.NewGuid().ToString();
        }

        [SerializeField] private BattleData battleData;
        [SerializeField] private List<Obstacle> obstacles;
    
        [Header("Visuals")]
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
                foreach (Obstacle obstacle in obstacles) {
                    SaveManager.Instance.ChangeObstacleData(obstacle.GetID(), false);
                }
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
}
