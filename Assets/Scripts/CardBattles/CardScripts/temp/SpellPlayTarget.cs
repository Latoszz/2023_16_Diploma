using System;
using CardBattles.Character;
using CardBattles.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace CardBattles.CardScripts.temp {
    [RequireComponent(typeof(Canvas),typeof(Image))]

    public class SpellPlayTarget : MonoBehaviour,ICardPlayTarget, IDropHandler, IPointerEnterHandler,IPointerExitHandler {
        private Image image;
        private Canvas canvas;
        
        private bool isActive = false;

        private bool IsActive {
            set {
                var alpha = 0f;
                if (value) {
                    alpha = 0.5f;
                }
                image.DOColor(new Color(0.5f,0.5f,0.5f,alpha), 0.1f).SetEase(Ease.InOutQuad).SetLink(this.gameObject, LinkBehaviour.KillOnDestroy);;
                
                canvas.overrideSorting = value;
                isActive = value;
                
            }
            get => isActive;
        }

        private void Awake() {
            image = GetComponent<Image>();
            canvas = GetComponent<Canvas>();
        }

        public void OnDrop(PointerEventData eventData) {
            if (!IsActive) {
                return;
            }
            
            if (eventData.pointerDrag is null) {
                Debug.Log($"{name}, was dropped a null object");
                return;
            }
            if (!eventData.pointerDrag.TryGetComponent
                    (typeof(Spell), out var draggedCard)) {
                Debug.Log($"{name}, dropped object was not a card");
                return;
            }
            CharacterManager.PlayACard((Card)draggedCard, this);
            IsActive = false;
        }

        //TODO MOVE TO NEW CLASS
        //TODO MAGIC NUMBER AND COLOR AND EASE
        public void OnPointerEnter(PointerEventData eventData) {
            if(eventData.pointerDrag is null)
                return;
            if(!eventData.pointerDrag.TryGetComponent(typeof(Spell), out var spell))
                return;
            
            IsActive = true;
        }
        //TODO SAME AS PREV
        public void OnPointerExit(PointerEventData eventData) {
            if(!IsActive)
                return;
            IsActive = false;
            image.DOColor(new Color(0.5f,0.5f,0.5f,0.0f), 0.1f).SetEase(Ease.InOutQuad).SetLink(this.gameObject, LinkBehaviour.KillOnDestroy);;
        }
    }
}