using CardBattles.CardScripts.CardDatas;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardBattles.CardScripts.Additional {
    public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [Foldout("Card scale"), SerializeField]
        public static float scaleInDeck = 0.8f;

        [Foldout("Card scale"), SerializeField]
        public static float scaleInHand = 0.9f;

        [Foldout("Card scale"), SerializeField]
        public static float scaleOnBoard = 1f;

        [Foldout("Card scale"), SerializeField]
        public static float scaleOnHover = 1.1f;

        [InfoBox("All the other scales are static, TODO make them visible here, to edit them edit code  ")]
        private float currentScale = 1f;

        [Header("Front/Back")] [Foldout("Objects")] [SerializeField]
        private CanvasGroup frontOfCard;

        [Foldout("Objects")] [SerializeField]
        private Image backOfCard;

        [Space, Header("Elements")] [Foldout("Objects")] [SerializeField]
        private TextMeshProUGUI cardName;

        [Foldout("Objects")] [SerializeField]
        private TextMeshProUGUI description;

        [Foldout("Objects")] [SerializeField]
        private TextMeshProUGUI attack;

        [Foldout("Objects")] [SerializeField]
        private TextMeshProUGUI health;

        [Foldout("Objects")] [SerializeField]
        private Image cardImage;

        [Foldout("Objects")] [SerializeField]
        private Image cardSetSymbolBox;

        [Foldout("Objects")] [SerializeField]
        private Image cardSetSymbol;

        [Foldout("Objects")] [SerializeField]
        private Image cardType;

        [Foldout("Objects")] [SerializeField]
        private CanvasGroup minionOnlyElements;

        public bool frontVisible;

        private void Awake() {
            frontOfCard.enabled = !frontVisible;
            backOfCard.enabled = !frontVisible;
        }

        public void SetCardDisplayData(CardData cardData) {
            cardImage.sprite = cardData.sprite;
            cardName.text = cardData.name;
            description.text = cardData.description;

            cardSetSymbol.sprite = cardData.cardSet.cardSetIcon;
            cardSetSymbolBox.color = cardData.cardSet.setColor;

            switch (cardData) {
                case MinionData minionData:
                    SetMinionDisplayData(minionData);
                    break;
                case SpellData spellData:
                    SetSpellDisplayData(spellData);
                    break;
            }
        }

        private void SetMinionDisplayData(MinionData minionData) {
            attack.text = minionData.attack.ToString();
            health.text = minionData.maxHealth.ToString();
        }

        private void SetSpellDisplayData(SpellData spellData) {
            minionOnlyElements.GetComponent<CanvasGroup>().alpha = 0;
        }

        public void ChangeCardVisible(bool visible) {
            frontOfCard.enabled = !visible;
            backOfCard.enabled = !visible;
            frontVisible = visible;
            transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        }

        public void ChangeCardVisible() {
            frontOfCard.enabled = !frontVisible;
            backOfCard.enabled = !frontVisible;
            frontVisible = !frontVisible;
        }

        internal void UpdateData(int newAttack, int newCurrentHealth, int newMaxHealth) {
            UpdateStat(attack, newAttack);
            UpdateStat(health, newCurrentHealth);
         

            //TODO Make it red when this is true
            if (newCurrentHealth != newMaxHealth) {
                health.color = new Color(0.8f,0.3f,0.4f);
            } else {
                health.color = Color.white;
            }
        }

        [SerializeField, Foldout("UpdateStat")]
        private float growScale = 1.5f;
        [SerializeField, Foldout("UpdateStat")]
        private float growDuration = 0.2f;
        [SerializeField, Foldout("UpdateStat")]
        private float shrinkDuration = 0.3f;
        private void UpdateStat(TextMeshProUGUI textComponent, int newValue) {
            if (textComponent.text != newValue.ToString()) {
                Vector3 originalScale = textComponent.transform.localScale;

                Sequence scaleSequence = DOTween.Sequence();
                scaleSequence.Append(textComponent.transform.DOScale(originalScale * growScale, growDuration));
                scaleSequence.AppendCallback(() => {
                    textComponent.text = newValue.ToString();
                });
                scaleSequence.Append(textComponent.transform.DOScale(originalScale, shrinkDuration));
                scaleSequence.Play();
            }
        }

        [SerializeField] private float scaleOnHoverTime = 0.1f;

        public void OnPointerEnter(PointerEventData eventData) {
            //TODO add ability to drop some cards on top of other, like a buff
            if (!frontVisible || eventData.pointerDrag is not null) {
                return;
            }

            transform.DOScale(scaleOnHover,
                scaleOnHoverTime);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (!frontVisible || eventData.pointerDrag is not null) {
                return;
            }

            transform.DOScale(Vector3.one * currentScale, scaleOnHoverTime);
        }

        public void IsPlacedOnBoard(bool isNotNull) {
            if (!isNotNull) {
                ChangeCurrentScale(1f);
                return;
            }

            ChangeCurrentScale(scaleOnBoard);
        }

        public void IsDrawn() {
            ChangeCurrentScale(scaleInHand);
        }

        private void ChangeCurrentScale(float scale) {
            currentScale = scale;
            transform.DOScale(currentScale, 0.3f);
        }
    }
}