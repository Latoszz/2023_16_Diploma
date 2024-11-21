using NPCScripts;
using UnityEngine;

namespace Interaction.Objects {
    public class ChangeDialogueInteract : InteractableObject {
        [SerializeField] private TalkableNPC npc;
        public override void Interact() {
            npc.SetUpNextDialogue();
        }
    }
}
