using Events;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialItem : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private Item item;
        [Range(0, 10)] 
        [SerializeField] private float detectionDistance = 8;

        private GameObject player;

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if(Vector3.Distance(player.transform.position, transform.position) > detectionDistance)
                return;
            GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
        }
    }
}
