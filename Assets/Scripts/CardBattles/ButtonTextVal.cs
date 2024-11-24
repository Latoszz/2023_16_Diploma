using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardBattles {
    public class ButtonTextVal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Sprite imageOn;
        [SerializeField] private Sprite imageInBetween;
        [SerializeField] private Sprite spriteOff;
        [SerializeField] public Button button;
        [SerializeField] public Text text;
        private bool hoveringOver = false;
        public RectTransform rectTransformText;
        private List<Vector3> textPositions = new List<Vector3>();
        private bool isOverriden = false;
        
        [SerializeField] public float textMoveDownAmount = 3f;
        
        private void Awake() {
            textPositions = new List<Vector3>();
            rectTransformText = text.GetComponent<RectTransform>();
            var textPosition = rectTransformText.localPosition;
            textPositions.Add(textPosition);
            textPositions.Add(textPosition + Vector3.down * textMoveDownAmount/2f);
            textPositions.Add(textPosition + Vector3.down * textMoveDownAmount);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            StopAllCoroutines();
            hoveringOver = true;
            StartCoroutine(HoverOverSprite());
        }

        private void UpdateTextHeight() {
            if (isOverriden) {
                rectTransformText.localPosition = textPositions[2];
                return;
            }

            if (button.image.sprite == imageInBetween) {
                rectTransformText.localPosition = textPositions[1];
            }
            else {
                rectTransformText.localPosition = textPositions[0];
            }

        }
        
        public void OverrideButtonSprite( bool turnOn) {
            isOverriden = !turnOn;
            button.image.overrideSprite = turnOn ? null : spriteOff;
            UpdateTextHeight();
        }
        private IEnumerator HoverOverSprite() {
            while (hoveringOver) {
                
                if (hoveringOver && button.IsInteractable() && button.isActiveAndEnabled){
                    button.image.sprite = imageInBetween;
                }
                else {
                    button.image.sprite = imageOn;
                }
                UpdateTextHeight();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            hoveringOver = false;
            button.image.sprite = imageOn;
            UpdateTextHeight();
            StopAllCoroutines();
        }
    }
}