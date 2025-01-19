using NPCScripts;
using UnityEngine;

namespace Interaction {
    public class DisappearOffScreen : MonoBehaviour {
        [SerializeField] private TalkableNPC npc;
        [SerializeField] private GameObject item;

        private void OnBecameInvisible() {
            if (npc.TalkedTo()) {
                gameObject.SetActive(false);
                if (item is not null) {
                    item.SetActive(true);
                }
            }
        }
    }
}
