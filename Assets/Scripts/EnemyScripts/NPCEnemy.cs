using EnemyScripts;
using Interaction.Objects;
using NPCScripts;
using SaveSystem;
using UnityEngine;

public class NPCEnemy : TalkableEnemy {
    [SerializeField] private TalkableNPC npc;
    
    private void Start() {
        if(state == EnemyState.Defeated)
            gameObject.SetActive(false);
    }
        
    public override void ChangeState(EnemyState state) {
        this.state = state;
        SaveManager.Instance.ChangeEnemyData(enemyID, state);
        if (state == EnemyState.Defeated) {
            foreach (Obstacle obstacle in obstacles) {
                SaveManager.Instance.ChangeObstacleData(obstacle.GetID(), false);
            }
            SaveManager.Instance.ChangeNPCData(npc.GetID(), true);
        }
    }
}
