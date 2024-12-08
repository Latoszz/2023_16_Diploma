using Interaction.Objects;
using SaveSystem;

namespace EnemyScripts {
    public class DisappearingEnemy : TalkableEnemy {
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
                this.gameObject.SetActive(false);
            }
        }
    }
}
