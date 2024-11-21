using System;
using System.Collections.Generic;
using CardBattles.CardGamesManager;
using Interaction.Objects;
using SaveSystem;
using UnityEngine;

namespace EnemyScripts {
    public class Enemy : MonoBehaviour {
        [SerializeField] protected string enemyID;
        [ContextMenu("Generate guid for id")]
        protected void GenerateGuid() {
            enemyID = Guid.NewGuid().ToString();
        }

        [SerializeField] protected BattleData battleData;
        [SerializeField] protected List<Obstacle> obstacles;

        protected EnemyState state = EnemyState.Locked;
    
        public virtual void ChangeState(EnemyState state) {
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
