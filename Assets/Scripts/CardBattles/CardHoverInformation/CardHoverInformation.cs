using System;
using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts;
using CardBattles.Enums;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattles.CardHoverInformation {
    public class CardHoverInformation : MonoBehaviour {
        public static CardHoverInformation Instance;

        [SerializeField] private GameObject propertyBoxPrefab;
        private Transform propertiesContainer;
        private List<GameObject> activePropertyBoxes = new List<GameObject>();

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
        }

        private void Start() {
            propertiesContainer = transform;
        }

        private bool isDisplaying = false;
        private Card currentCard;

        private void OnEnable() {
            CardHoverOverScript.pointerEnterEvent.AddListener(PointerEventHandler);
        }

        private void OnDisable() {
            CardHoverOverScript.pointerEnterEvent.RemoveListener(PointerEventHandler);
        }

        private void PointerEventHandler(Card card, bool val) {
            if (card == currentCard) {
                if (val)
                    return;
                ClearPropertyBoxes();
                currentCard = null;
                return;
            }

            ClearPropertyBoxes();
            if (!ConditionsMet(card))
                return;

            currentCard = card;
            StartCoroutine(ShowInfo());
        }

        [SerializeField] private float secondsBeforeAdditionalInfoDispay = 1f;
        private int tmp = 0;
        
        private IEnumerator ShowInfo() {
            isDisplaying = true;
            var x = tmp;
            yield return new WaitForSeconds(secondsBeforeAdditionalInfoDispay);
            if(!isDisplaying || x < tmp)
                yield break;
            ShowCardDetails();
        }

        

        private void ShowCardDetails() {
            isDisplaying = true;
            tmp = 0;
            ClearPropertyBoxes();
            if(currentCard is null) return;
            CreateBoxes();
            FadeInBoxes();
        }
        [SerializeField] private Vector3 firstOffset = new Vector3(-20, 30, 0);
        [SerializeField] private float yOffsetBetween = 50f;
        private void CreateBoxes() {
            var card = currentCard;
            var firstPosition = card.transform.position + firstOffset;
            for (var i = 0; i < card.properties.Count; i++) {
                var property = card.properties[i];
                var propertyBox = Instantiate(propertyBoxPrefab, propertiesContainer, false);
                propertyBox.transform.position = firstPosition + Vector3.down * (i * ( yOffsetBetween + propertyBox.GetComponent<RectTransform>().sizeDelta[1]/2));
                var description = CreateDescription(property);

                var texts = propertyBox.GetComponentsInChildren<Text>();
                texts[0].text = description[0];
                texts[1].text = description[1];
                
                
                activePropertyBoxes.Add(propertyBox);
            }
        }

        
        private List<string> CreateDescription(AdditionalProperty property) {
            var output = new List<string> {
                property.ToString(),
                AdditionalPropertyHelper.GetDescription(property)
            };
            return output;
        }
        
        [SerializeField] private float fadeInTime = 0.25f;
        [SerializeField] private Ease fadeInEase = Ease.InOutCubic;

        private void FadeInBoxes(bool val = true) {
            
            int value = val ? 1 : 0;
            foreach (var propertybox in activePropertyBoxes) {
                var tween = propertybox.GetComponent<CanvasGroup>()
                    .DOFade(value, fadeInTime)
                    .SetEase(fadeInEase)
                    .SetLink(propertybox, LinkBehaviour.KillOnDestroy);
                tween.Play();
            }
        }
        private bool ConditionsMet(Card card) {
            if (!card.FrontVisible) return false;
            return true; // Placeholder; replace with your conditions
        }

        private void ClearPropertyBoxes() {
            // Destroy all active property boxes
            foreach (var box in activePropertyBoxes) {
                box.GetComponent<CanvasGroup>()
                    .DOFade(0, 0.1f)
                    .OnComplete(() => Destroy(box));
            }
            activePropertyBoxes.Clear();
            isDisplaying = false;
            tmp += 1;

        }

        public void HideCardDetails() {
            ClearPropertyBoxes();
            isDisplaying = false;
        }
    }
}