using Events;
using InputScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class ClickPoint : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject visual;
        
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnActivateClickPoint += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnActivateClickPoint -= Activate;
        }

        private void Activate() {
            visual.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData) {
            MouseInputManager.Instance.ForceSetTargetPoint(transform.position);
            TutorialDialogue.Instance.DisplayNextSentence();
            visual.SetActive(false);
        }
    }
}
