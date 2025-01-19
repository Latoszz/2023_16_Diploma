using System.Collections;
using Events;
using UI.Dialogue;
using UnityEngine;

namespace UI.Infos {
    public class RewardInfoPopup: InfoPopup {
        private void OnEnable() {
            GameEventsManager.Instance.ItemEvents.OnItemReward += ShowItemInfo;
        }
    
        private void OnDisable() {
            GameEventsManager.Instance.ItemEvents.OnItemReward -= ShowItemInfo;
        }

        private void ShowItemInfo(string itemName) {
            StartCoroutine(ShowItemInfoCoroutine(itemName));
        }
        
        private IEnumerator ShowItemInfoCoroutine(string itemName) {
            yield return new WaitUntil((() => DialogueController.Instance.DialogueClosed));
            infoTitle.text = "You got a new item!";
            infoDescription.text = $"{itemName} has been added to your Inventory";
            PanelFadeIn(0f, -310f);
            StartCoroutine(DisappearAfterSecondsInt(secondsToDisappearInt, 600f, -310f));
        }
    }
}