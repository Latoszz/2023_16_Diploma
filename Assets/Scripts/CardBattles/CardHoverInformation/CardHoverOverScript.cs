using System;
using System.Collections;
using CardBattles.CardScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardBattles.CardHoverInformation {
    public class CardHoverOverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private Card card;
        public static UnityEvent<Card, bool> pointerEnterEvent = new UnityEvent<Card, bool>();

        private void Awake() {
            card = GetComponent<Card>();
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            pointerEnterEvent?.Invoke(card, true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            pointerEnterEvent?.Invoke(card, false);
        }
    }
}