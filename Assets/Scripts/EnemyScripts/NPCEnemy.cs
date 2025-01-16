using EnemyScripts;
using Interaction.Objects;
using NPCScripts;
using SaveSystem;
using UnityEngine;

public class NPCEnemy : TalkableEnemy {
    [SerializeField] private TalkableNPC npc;
    
    private void Start() {
        switch (state) {
            case EnemyState.Defeated: {
                gameObject.SetActive(false);
                break;
            }
            case EnemyState.Undefeated:
                battleIndicator.ShowIcon();
                break;
            default:
                battleIndicator.HideIcon();
                break;
        }
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
