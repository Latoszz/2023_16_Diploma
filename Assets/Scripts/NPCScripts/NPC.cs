using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NPCScripts {
    public abstract class NPC : MonoBehaviour, IInteractable, IPointerClickHandler {
        
        public void OnPointerClick(PointerEventData eventData) {
            Interact();
        }
        
        public abstract void Interact();
    }
}