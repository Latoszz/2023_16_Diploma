using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class UnlockStatueClick: MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            GameEventsManager.Instance.TutorialEvents.UnlockStatue();
        }
    }
}