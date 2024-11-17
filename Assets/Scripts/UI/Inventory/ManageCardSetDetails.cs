using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManageCardSetDetails : MonoBehaviour {
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject cardListSpace;
    [SerializeField] private GameObject cardSetDetailPrefab;
    [SerializeField] private CardItemPool cardItemPool;

    private List<GameObject> cards = new List<GameObject>();
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", false);
    }

    public void ReadCardSet(CardSetData cardSetData) {
        ClearObjects();
        nameText.text = cardSetData.displayName;

        foreach (CardData cardData in cardSetData.cards) {
            SetUpObjects(cardData);
        }
        DisplayCards();
    }

    private void DisplayCards() {
        animator.SetBool("isOpen", true);
    }
    
    private void SetUpObjects(CardData cardData) {
        GameObject displayObject = cardItemPool.GetCardItem();
        displayObject.transform.SetParent(cardListSpace.transform, true);
        displayObject.GetComponent<CardDetail>().SetUpCardDetails(cardData);
        displayObject.GetComponent<ShowCardDetails>().SetUpData(cardData);
        cards.Add(displayObject);
    }

    private void ClearObjects() {
        foreach (GameObject cardObject in cards) {
            cardItemPool.ReturnCardItem(cardObject);
        }
        cards.Clear();
        nameText.text = "";
    }

    public void Hide() {
        animator.SetBool("isOpen", false);
    }
}