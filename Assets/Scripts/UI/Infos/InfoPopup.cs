using System.Collections;
using DG.Tweening;
using Events;
using QuestSystem;
using TMPro;
using UnityEngine;

public class InfoPopup : MonoBehaviour {
    [Header("Time")] 
    [SerializeField] private int secondsToDisappearInt = 5;
    [SerializeField] private float fadeTime = 1f;
    
    [Header("Setup")]
    [SerializeField] private RectTransform infoPopupPanel;
    [SerializeField] private TMP_Text infoTitle;
    [SerializeField] private TMP_Text infoDescription;
    [SerializeField] private QuestManager questManager;
    
    private void OnEnable() {
        GameEventsManager.Instance.QuestEvents.OnStartQuest += ShowInfo;
    }
    
    private void OnDisable() {
        GameEventsManager.Instance.QuestEvents.OnStartQuest -= ShowInfo;
    }

    private void ShowInfo(string questId) {
        StartCoroutine(ShowInfoCoroutine(questId));
    }

    private IEnumerator ShowInfoCoroutine(string questId) {
        yield return new WaitUntil((() => DialogueController.Instance.DialogueClosed));
        QuestInfoSO questInfo = questManager.GetQuestById(questId).info;
        infoTitle.text = questInfo.displayName;
        infoDescription.text = questInfo.questDescription;
        PanelFadeIn();
        StartCoroutine(DisappearAfterSecondsInt(secondsToDisappearInt));
    }
    
    private void PanelFadeIn() {
        infoPopupPanel.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.InOutQuint);
    }

    private void PanelFadeOut() {
        infoPopupPanel.DOAnchorPos(new Vector2(600f, 0f), fadeTime, false).SetEase(Ease.InOutQuint);
    }

    private IEnumerator DisappearAfterSecondsInt(int seconds) {
        yield return new WaitForSeconds(seconds);
        PanelFadeOut();
    }
}
