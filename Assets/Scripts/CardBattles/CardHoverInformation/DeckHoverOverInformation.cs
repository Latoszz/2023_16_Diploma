using System;
using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts;
using CardBattles.Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeckHoverOverInformation : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    private DeckManager deck;
    [SerializeField] private GameObject box;
    [SerializeField] private Text amountText;

    private bool isDisplaying;
    [SerializeField] private float secondsBeforeAdditionalInfoDispay = 1f;
    private int tmp = 0;

    private void Awake() {
        if (!TryGetComponent(typeof(DeckManager), out var deckManager))
            return;
        deck = (DeckManager)deckManager;
    }
    
    private IEnumerator ShowInfo() {
        isDisplaying = true;
        var x = tmp;
        yield return new WaitForSeconds(secondsBeforeAdditionalInfoDispay);
        if (!isDisplaying || x < tmp)
            yield break;
        ShowDeckDetails();
    }

    private void ShowDeckDetails() {
        isDisplaying = true;
        tmp = 0;
        ClearPropertyBoxes();
        MoveBox();
        FadeInBoxes();
    }

    [SerializeField] private float yOffsetBetween = 50f;


    private void MoveBox() {
        box.transform.localPosition = Vector3.zero;
        box.transform.position = transform.position + new Vector3(0, yOffsetBetween, 0);
        amountText.text = deck.CardAmount.ToString();
    }

    [SerializeField] private float fadeInTime = 0.25f;
    [SerializeField] private Ease fadeInEase = Ease.InOutCubic;

    private void FadeInBoxes(bool val = true) {
        int value = val ? 1 : 0;
        var tween = box.GetComponent<CanvasGroup>()
            .DOFade(value, fadeInTime)
            .SetEase(fadeInEase)
            .SetLink(box, LinkBehaviour.KillOnDestroy);
        tween.Play();
    }

    private void ClearPropertyBoxes() {
        box.GetComponent<CanvasGroup>().alpha = 0;
        box.transform.position = new Vector3(3000, 3000, 3000);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(!isDisplaying)
            StartCoroutine(ShowInfo());
    }

    public void OnPointerExit(PointerEventData eventData) {
        ClearPropertyBoxes();
        isDisplaying = false;
    }
}