using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public abstract class InteractableObject : MonoBehaviour, IInteractable, IPointerClickHandler{
        public abstract void Interact();
    
        public void OnPointerClick(PointerEventData eventData) {
            Interact();
        }
    }
}
