using Audio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardBattles.CardScripts.Additional {
    public class CardDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector2 originalPosition;
        public bool droppedOnSlot;
        private Card card;
        private Canvas canvas;
        private int baseOrder;

        [BoxGroup("make it pretty")] [SerializeField]
        private float alphaDuringDragging = 0.8f;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            card = GetComponent<Card>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!enabled || !card.IsPlayers || droppedOnSlot) {
                enabled = false;
                return;
            }

            baseOrder = canvas.sortingOrder;
            canvas.sortingOrder = 100;
            

            canvasGroup.alpha = alphaDuringDragging;
            canvasGroup.blocksRaycasts = false;
            originalPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData) {
            if (!enabled) {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPointerPosition
            );
            rectTransform.anchoredPosition = localPointerPosition;
        }

        [SerializeField] private float snapBackTime = 0.1f;
        [SerializeField] private float animationFasterIfDistanceLessThan = 700f;

        public void OnEndDrag(PointerEventData eventData) {
            if (!enabled) {
                return;
            }

            canvas.sortingOrder = baseOrder;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            if (droppedOnSlot) {
                enabled = false;
            }
            else {
                 SnapBack();
            }
        }

        private void SnapBack() {
            var distance = Vector3.Distance(transform.position, originalPosition);
            var animationTime = snapBackTime;
            if (distance < animationFasterIfDistanceLessThan) {
                animationTime *= distance / animationFasterIfDistanceLessThan;
            }

            transform
                .DOMove(originalPosition,
                    animationTime)
                .SetEase(Ease.InOutSine);
        }
    }
}