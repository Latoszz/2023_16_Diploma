using System;
using Events;
using QuestSystem;
using UnityEngine;


public class GiveItemWithIdQuestStep: QuestStep {
    [SerializeField] private string itemToGiveId;
    private bool isGiven;
    
    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdGiven += ItemGiven;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdGiven -= ItemGiven;
    }

    private void ItemGiven(string itemId) {
        if (itemId == itemToGiveId) {
            UpdateState();
            FinishQuestStep();
        }
    }

    private void UpdateState() {
        string state = isGiven.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        isGiven = Convert.ToBoolean(state);
        UpdateState();
    }
    
    protected override void InitializeQuestStepState() {
        UpdateState();
    }
}
