using CardBattles.CardScripts.CardDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory {
    public class CardDetail : MonoBehaviour{
        public CardData cardData;

        [SerializeField] private Image cardSprite;
        [SerializeField] private TMP_Text cardNameText;

        public void SetUpCardDetails(CardData cardData) {
            this.cardData = cardData;
            cardSprite.sprite = cardData.sprite;
            cardNameText.text = cardData.cardName;
        }
    }
}