using Events;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class UnlockQuests: MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            GameEventsManager.Instance.TutorialEvents.UnlockQuests();
        }

        public void Unlock() {
            if (TutorialUIManager.Instance.QuestsUnocked)
                return;
            GameEventsManager.Instance.TutorialEvents.UnlockQuests();
        }
    }
}