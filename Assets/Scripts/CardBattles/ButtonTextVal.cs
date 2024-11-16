using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardBattles {
    public class ButtonTextVal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Sprite imageOn;
        [SerializeField] private Sprite imageInBetween;
        [SerializeField] public Button button;
        [SerializeField] public Text text;
        private bool hoveringOver = false;
        public RectTransform rectTransformText;

        private void Awake() {
            rectTransformText = text.GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            StopAllCoroutines();
            hoveringOver = true;
            StartCoroutine(HoverOverSprite());
        }

        private IEnumerator HoverOverSprite() {
            while (hoveringOver) {
                if (hoveringOver && button.IsInteractable() && button.isActiveAndEnabled){
                    button.image.sprite = imageInBetween;
                }
                else {
                    button.image.sprite = imageOn;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            hoveringOver = false;
            button.image.sprite = imageOn;
            StopAllCoroutines();
        }
    }
}