using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class PlayerClickPoint: MonoBehaviour, IPointerClickHandler{
        [SerializeField] private GameObject visual;
        private void Start() {
            StartCoroutine(ShowClickPoint());
        }
        
        private IEnumerator ShowClickPoint() {
            yield return new WaitForSeconds(1);
            StartCoroutine(Show());
        }

        private IEnumerator Show() {
            yield return new WaitUntil(() => !TutorialDialogue.Instance.IsOpen);
            visual.SetActive(true);
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (!visual.activeSelf)
                return;
            visual.SetActive(false);
        }
    }
}