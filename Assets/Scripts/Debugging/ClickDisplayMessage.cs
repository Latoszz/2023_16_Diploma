using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDisplayMessage: MonoBehaviour, IPointerClickHandler{
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(gameObject.name);
    }
}