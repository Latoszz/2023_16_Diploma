using Events;
using QuestSystem;
using UnityEngine;

public class ActivateOnQuestStart : MonoBehaviour {
    [SerializeField] private QuestInfoSO questInfoSo;
    [SerializeField] private GameObject objectToActivate;

    private void OnEnable() {
        GameEventsManager.Instance.QuestEvents.OnStartQuest += Activate;
    }

    private void OnDisable() {
        GameEventsManager.Instance.QuestEvents.OnStartQuest -= Activate;
    }

    private void Activate(string questId) {
        if (questId == questInfoSo.id) {
            objectToActivate.SetActive(true);
        }
    }
}
