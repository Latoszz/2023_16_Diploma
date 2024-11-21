using UnityEngine;
using UnityEngine.EventSystems;

namespace Debugging {
    public class ClickDisplayMessage: MonoBehaviour, IPointerClickHandler{
        public void OnPointerClick(PointerEventData eventData) {
            Debug.Log(gameObject.name);
        }
    }
}