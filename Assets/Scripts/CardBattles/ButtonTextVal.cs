using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardBattles {
    
    public class ButtonTextVal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        
        
        [BoxGroup("Sprite")]
        [SerializeField] private Sprite spriteOn;
        [BoxGroup("Sprite")]
        [SerializeField] private Sprite spriteInBetween;
        [BoxGroup("Sprite")]
        [SerializeField] private Sprite spriteOff;
        private Button button;
        private Text text;
        
        [HideInInspector]
        public RectTransform rectTransformText;
        
        
        
        private List<Vector3> textPositions = new List<Vector3>();
        private bool isOverriden = false;
        private bool hoveringOver = false;
        [SerializeField] public float textMoveDownAmount = 3f;

        private void Awake() {
            SetComponents();
            SetTextPositions();
        }

        private void SetComponents() {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            rectTransformText = text.GetComponent<RectTransform>();

        }
        private void SetTextPositions() {
            textPositions = new List<Vector3>();
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

            if (button.image.sprite == spriteInBetween) {
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
                    button.image.sprite = spriteInBetween;
                }
                else {
                    button.image.sprite = spriteOn;
                }
                UpdateTextHeight();
                yield return new WaitForSeconds(0.1f);
            }
        }

        
            
        public void OnPointerExit(PointerEventData eventData) {
            hoveringOver = false;
            button.image.sprite = spriteOn;
            UpdateTextHeight();
            StopAllCoroutines();
        }

        public void ResetPosition() {
            button.image.sprite = spriteOn;
            UpdateTextHeight();
        }
    }
}