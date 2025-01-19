using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class UnlockStatueClick: MonoBehaviour, IPointerClickHandler {
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;

        private GameObject player;

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start() {
            
        }

        public void OnPointerClick(PointerEventData eventData) {
            if(Vector3.Distance(player.transform.position, transform.position) > detectionDistance)
                return;
            GameEventsManager.Instance.TutorialEvents.UnlockStatue();
            this.enabled = false;
        }
    }
}