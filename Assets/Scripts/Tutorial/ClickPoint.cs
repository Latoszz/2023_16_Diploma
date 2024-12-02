using InputScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class ClickPoint : MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            MouseInputManager.Instance.ForceSetTargetPoint(transform.position);
            TutorialDialogue.Instance.HideDialogue();
            gameObject.SetActive(false);
        }
    }
}
