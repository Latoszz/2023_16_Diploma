using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction {
    public class ShowOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [Range(0, 10)]
        [SerializeField] private float detectionDistance = 8;
    
        private Outline outlineScript;
        private GameObject player;

        private void Awake() {
            outlineScript = GetComponent<Outline>();
            outlineScript.enabled = false;
            player = GameObject.FindGameObjectWithTag("Player");
        }
    
        public void OnPointerEnter(PointerEventData eventData) {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                outlineScript.enabled = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            outlineScript.enabled = false;
        }
    }
}
