using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class CheckButtonClicked: MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            GameEventsManager.Instance.TutorialEvents.ButtonClicked(name);
        }
    }
}