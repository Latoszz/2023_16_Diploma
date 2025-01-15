using EnemyScripts;
using NPCScripts;
using UnityEngine;

namespace Interaction {
    public class NextDialogueTrigger : MonoBehaviour {
        [SerializeField] private GameObject talkable;
        private Collider talkableCollider;
        private TalkableNPC talkableNPC;
        private TalkableEnemy talkableEnemy;

        private void Start() {
            talkableCollider = talkable.GetComponent<Collider>();
            talkableNPC = talkable.GetComponent<TalkableNPC>();
            talkableEnemy = talkable.GetComponent<TalkableEnemy>();
        }
        
        public void OnTriggerEnter(Collider other) {
            if (other != talkableCollider) return;
            
            if (talkableNPC is not null) {
                talkableNPC.SetUpNextDialogue();
            }
            else if (talkableEnemy is not null) {
                talkableEnemy.SetUpNextDialogue();
            }
        }
    }
}
