using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class UnlockItem: MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject item;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;

        private GameObject player;

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if(Vector3.Distance(player.transform.position, transform.position) > detectionDistance)
                return;
            item.SetActive(true);
        }
    }
}