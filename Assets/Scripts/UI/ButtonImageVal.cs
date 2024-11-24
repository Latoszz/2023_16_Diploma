using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI {
    public class ButtonImageVal: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Sprite imageOn;
        [SerializeField] private Sprite imageInBetween;
        [SerializeField] private Sprite spriteOff;
        [SerializeField] public Button button;
        [SerializeField] public Image image;
        private bool hoveringOver = false;
        public RectTransform rectTransformImage;
        private List<Vector3> imagePositions = new List<Vector3>();
        private bool isOverriden = false;
        
        [SerializeField] public float textMoveDownAmount = 3f;
        
        private void Awake() {
            imagePositions = new List<Vector3>();
            rectTransformImage = image.GetComponent<RectTransform>();
            var textPosition = rectTransformImage.localPosition;
            imagePositions.Add(textPosition);
            imagePositions.Add(textPosition + Vector3.down * textMoveDownAmount/2f);
            imagePositions.Add(textPosition + Vector3.down * textMoveDownAmount);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            StopAllCoroutines();
            hoveringOver = true;
            StartCoroutine(HoverOverSprite());
        }

        private void UpdateTextHeight() {
            if (isOverriden) {
                rectTransformImage.localPosition = imagePositions[2];
                return;
            }

            if (button.image.sprite == imageInBetween) {
                rectTransformImage.localPosition = imagePositions[1];
            }
            else {
                rectTransformImage.localPosition = imagePositions[0];
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