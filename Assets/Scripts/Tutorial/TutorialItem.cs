using Events;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialItem : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private Item item;
        
        public void OnPointerClick(PointerEventData eventData) {
            GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
        }
    }
}
