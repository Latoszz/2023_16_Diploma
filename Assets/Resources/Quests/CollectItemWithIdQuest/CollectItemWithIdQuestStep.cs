using System;
using Events;
using QuestSystem;
using UnityEngine;

public class CollectItemWithIdQuestStep: QuestStep {
    [SerializeField] private string itemToCollectId;
    private bool isCollected;
    
    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected += ItemCollected;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected -= ItemCollected;
    }

    private void ItemCollected(string itemId) {
        if (itemId == itemToCollectId) {
            UpdateState();
            FinishQuestStep(); 
        }
    }

    private void UpdateState() {
        string state = isCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        isCollected = Convert.ToBoolean(state);
        UpdateState();
    }
}