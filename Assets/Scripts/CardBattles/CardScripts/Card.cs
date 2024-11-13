using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CardBattles.CardScripts.Additional;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.CardScripts {
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public abstract class Card : PlayerEnemyMonoBehaviour, IHasCost {
        [NonSerialized] private EffectManager.EffectDelegate effectDelegate;
        [NonSerialized] protected Canvas canvas;

        [NonSerialized] protected CardDisplay cardDisplay;
        [NonSerialized] protected CardAnimation cardAnimation;
        [NonSerialized] public CardDragging cardDragging;

        public bool FrontVisible {
            get {
                if (this.cardDisplay is null) return false;
                return this.cardDisplay.frontVisible;
            }
        }

        [BoxGroup("Card")] public string cardName;

        [BoxGroup("Card"), ResizableTextArea]
        public string description;

        [BoxGroup("Card"), ResizableTextArea]
        public string flavourText;

        [BoxGroup("Data"), Space]
        public List<AdditionalProperty> properties;

        [NonSerialized] public string cardSetName;

        [HorizontalLine(1f)] [BoxGroup("Data")]
        public TriggerEffectDictionary effectDictionary;

        [HorizontalLine(1f)] [CanBeNull]
        protected CardSpot isPlacedAt;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            cardDisplay = GetComponent<CardDisplay>();
            cardAnimation = GetComponent<CardAnimation>();
            cardDragging = GetComponent<CardDragging>();
        }

        public virtual void Initialize(CardData cardData, bool isPlayersCard) {
            SetTag(isPlayersCard);
            cardName = cardData.cardName;
            name = cardName;
            flavourText = cardData.flavourText;
            description = cardData.description;
            properties = cardData.properties.ToList();
            cardSetName = cardData.cardSet.name;
            effectDictionary = cardData.EffectDictionary;
        }

        public virtual int GetCost() {
            if (properties.Contains(AdditionalProperty.FreeToPlay)) {
                return 0;
            }

            if (properties.Contains(AdditionalProperty.Costly)) {
                return 2;
            }

            return 1;
        }


        //TODO CHANGE ITS NAME
        public void IsDrawn() {
            cardDragging.enabled = true;
            cardDisplay.IsDrawn();
        }

        public void AssignCardSpot(CardSpot cardSpot) {
            isPlacedAt = cardSpot;
            bool hasValue = cardSpot is not null;
            if (hasValue)
                cardSpot.card = this;

            cardDragging.droppedOnSlot = hasValue;
            cardDisplay.IsPlacedOnBoard(hasValue);
        }

        public void ChangeCardVisible(bool isPlayersVal) {
            cardDisplay.ChangeCardVisible(isPlayersVal);
        }

        public IEnumerator Move(Vector3 vector3) {
            yield return StartCoroutine(cardAnimation.MoveTo(vector3));
        }

        public IEnumerator DrawAnimation(Vector3 finalPosition) {
            yield return cardAnimation.DrawAnimation(finalPosition);
        }

        public virtual IEnumerator Play() {
            yield return StartCoroutine(cardAnimation.Play(this)); //it has the trigger inside
        }


        public IEnumerator DoEffect(EffectTrigger effectTrigger) {
            if (!effectDictionary.TryGetValue(effectTrigger, out var value))
                yield break;

            if (effectTrigger == EffectTrigger.OnPlay)
                yield return StartCoroutine(cardAnimation.OnPlayEffectDelay());
            var effectTargetValue = value;
            var targets = GetTargets(effectTargetValue.targetType);
            yield return StartCoroutine(
                EffectManager.effectDictionary[effectTargetValue.effectName](targets, effectTargetValue.value));
        }

        public IEnumerator ChangeSortingOrderTemporarily(int num) {
            canvas.sortingOrder += num;
            yield return new WaitForSeconds(2f);
            canvas.sortingOrder -= num;
        }

        private List<GameObject> GetTargets(TargetType targetType) {
            return BoardManager.Instance.GetTargets(targetType, this);
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }
}