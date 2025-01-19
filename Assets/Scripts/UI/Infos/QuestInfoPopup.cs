using System.Collections;
using Events;
using QuestSystem;
using UI.Dialogue;
using UnityEngine;

namespace UI.Infos {
    public class QuestInfoPopup: InfoPopup {
        [SerializeField] private QuestManager questManager;
        
        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += ShowQuestInfo;
        }
    
        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= ShowQuestInfo;
        }
        
        private void ShowQuestInfo(string questId) {
            StartCoroutine(ShowQuestInfoCoroutine(questId));
        }

        private IEnumerator ShowQuestInfoCoroutine(string questId) {
            yield return new WaitUntil((() => DialogueController.Instance.DialogueClosed));
            QuestInfoSO questInfo = questManager.GetQuestById(questId).info;
            infoTitle.text = questInfo.displayName;
            infoDescription.text = questInfo.questDescription;
            PanelFadeIn(0f, 0f);
            StartCoroutine(DisappearAfterSecondsInt(secondsToDisappearInt, 600f, 0f));
        }
    }
}