using EnemyScripts;
using UnityEngine;

namespace Interaction {
    public class UnlockEnemyTrigger : MonoBehaviour {
        [SerializeField] private GameObject collidingObject;
        [SerializeField] private Enemy enemy;
        private Collider objectCollider;
        private void Start() {
            objectCollider = collidingObject.GetComponent<Collider>();
        }
        
        public void OnTriggerEnter(Collider other) {
            if (other != objectCollider) return;
            enemy.ChangeState(EnemyState.Undefeated);
        }
    }
}
