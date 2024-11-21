using CardBattles.CardScripts.CardDatas;
using TMPro;
using UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.Inventory {
    public class ShowCardDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private float timeToWait = 0.5f;
        [SerializeField] private RectTransform descriptionWindow;
        [SerializeField] private TMP_Text descriptionText;
    
        private float hoverTimer = 0f;

        private CardData cardData;

        private bool isMouseOver;
        private Transform initialParent;
    
        private void Awake() {
            initialParent = transform.parent;
        }
    
        private void Update() {
            if (PauseManager.Instance.IsOpen)
                return;
        
            if (isMouseOver) {
                hoverTimer += Time.deltaTime;
                if (hoverTimer >= timeToWait) {
                    MoveTextNearCursor();
                    ShowDetails();
                }
            }
            else {
                hoverTimer = 0f;
                HideDetails();
            }
        }
    
        public void OnPointerEnter(PointerEventData eventData) {
            isMouseOver = true;
            descriptionWindow.transform.SetParent(transform.parent.transform.parent);
            descriptionWindow.transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData) {
            isMouseOver = false;
            descriptionWindow.transform.SetParent(initialParent);
        }
    
        private void MoveTextNearCursor() {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            descriptionWindow.sizeDelta = new Vector2(descriptionText.preferredWidth > 300 ?
                300 : descriptionText.preferredWidth, descriptionText.preferredHeight);
            descriptionWindow.transform.position = new Vector2(mousePosition.x + 20, mousePosition.y + descriptionWindow.sizeDelta.y/2);
        }

        private void ReadCardData() {
            string displayText = cardData.cardName + "";
            //displayText += "description text";
        
            if (cardData is MinionData) {
                displayText += "\nPower: " + ((MinionData)cardData).attack;
                displayText += "\nHealth: " + ((MinionData)cardData).maxHealth;
            }

            if (cardData is SpellData) {
                displayText += "\nSpell";
            }
        
            descriptionText.text = displayText;
            descriptionWindow.gameObject.SetActive(true);
        }

        public void SetUpData(CardData cardData) {
            this.cardData = cardData;
        }
    
        private void ShowDetails() {
            ReadCardData();
        }
    
        private void HideDetails() {
            descriptionWindow.gameObject.SetActive(false);
        }
    }
}