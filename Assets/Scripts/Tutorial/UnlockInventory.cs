using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class UnlockInventory: MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            GameEventsManager.Instance.TutorialEvents.UnlockInventory();
        }
    }
}