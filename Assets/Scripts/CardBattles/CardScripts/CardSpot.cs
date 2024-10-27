using System;
using Audio;
using CardBattles.Character;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardBattles.CardScripts {
    public class CardSpot : PlayerEnemyMonoBehaviour, ICardPlayTarget, IDropHandler, IPointerEnterHandler,
        IPointerExitHandler {
        private bool isActive = false;
        private Image image;
        public Card card;

        public static UnityEvent<CardSpot> highlight;

        private Color defaultColor = Color.clear;
        [SerializeField] private Color highlightColor = Color.cyan;

        [SerializeField] private static string cardsPlayClipName = "Cards.Place";

        private void Awake() {
            image = GetComponent<Image>();
            defaultColor = highlightColor;
            defaultColor.a = 0;
            StopHighlight();
        }


        public void OnDrop(PointerEventData eventData) {
            if (!isActive) {
                return;
            }

            if (eventData.pointerDrag is null) {
                Debug.Log($"{name}, was dropped a null object");
                return;
            }

            if (!eventData.pointerDrag.TryGetComponent
                    (typeof(Minion), out var draggedCard)) {
                if (draggedCard is not Minion) {
                    Debug.Log($"{name}, dropped object was not a card");
                    return;
                }
            }

            PlayDropSound();
            CharacterManager.PlayACard((Card)draggedCard, this);
            StopHighlight();
        }

        public static void PlayDropSound() {
            var placeClip = AudioCollection.Instance.GetClip(cardsPlayClipName);
            AudioManager.Instance.PlayWithVariation(placeClip);
        }

        //TODO MOVE TO NEW CLASS
        //TODO MAGIC NUMBER AND COLOR AND EASE
        public void OnPointerEnter(PointerEventData eventData) {
            if (!CanHighlight(eventData))
                return;
            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData) {
            StopHighlight();
        }

        private void Highlight() {
            highlight?.Invoke(this);
            isActive = true;
            image.DOColor(highlightColor, 0.1f).SetEase(Ease.InOutQuad);
        }

        private void StopHighlight() {
            isActive = false;
            image.DOColor(defaultColor, 0.1f).SetEase(Ease.InOutQuad);
        }

        private bool CanHighlight(PointerEventData eventData) {
            if (!IsPlayers)
                return false;

            if (!IsAvailable())
                return false;

            if (!HoldsPlayableMinion(eventData))
                return false;

            return true;
        }

        private bool HoldsPlayableMinion(PointerEventData eventData) {
            if (eventData.pointerDrag is not null &&
                !eventData.pointerDrag.TryGetComponent(typeof(Minion), out var minion)
               )
                if (minion is Minion)
                    return false;
            return true;
        }

        private bool IsAvailable() {
            return (card is null);
        }
    }
}