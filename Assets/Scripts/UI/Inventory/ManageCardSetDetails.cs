using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageCardSetDetails : MonoBehaviour {
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject cardListSpace;
    [SerializeField] private GameObject cardSetDetailPrefab;
    [SerializeField] private RectTransform descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;

    private List<CardData> cards = new List<CardData>();
    private Animator animator;
    private bool isOpen;
    public bool IsOpen => isOpen;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", false);
    }

    public void ReadCardSet(CardSetData cardSetData) {
        isOpen = true;
        nameText.text = cardSetData.displayName;

        foreach (CardData cardData in cardSetData.cards) {
            cards.Add(cardData);
        }
        DisplayCards();
    }

    private void DisplayCards() {
        foreach (CardData cardData in cards) {
           SetUpObjects(cardData);
        }
        animator.SetBool("isOpen", true);
    }
    
    private void SetUpObjects(CardData cardData) {
        GameObject displayObject = Instantiate(cardSetDetailPrefab, cardListSpace.transform, true);
        displayObject.AddComponent<CardDetail>().cardData = cardData;
        descriptionWindow = displayObject.transform.GetChild(2).GetComponent<RectTransform>();
        descriptionText = descriptionWindow.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        displayObject.AddComponent<ShowCardDetails>().SetComponents(descriptionWindow, descriptionText);
        displayObject.transform.GetChild(0).GetComponent<Image>().sprite = cardData.sprite;
        displayObject.transform.GetChild(1).GetComponent<TMP_Text>().text = cardData.cardName;
    }

    public void Hide() {
        animator.SetBool("isOpen", false);
        foreach (Transform displayObject in cardListSpace.transform) {
            Destroy(displayObject.gameObject);
        }
        cards.Clear();
        nameText.text = "";
        isOpen = false;
    }
}