using NPCScripts;
using UnityEngine;

namespace Interaction.Objects {
    public class ChangeDialogueInteract : InteractableObject {
        [SerializeField] private TalkableNPC npc;

        private void Start() {
            
        }
        
        public override void Interact() {
            npc.SetUpNextDialogue();
            this.enabled = false;
        }
    }
}
