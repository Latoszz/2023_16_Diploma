using Events;
using QuestSystem;
using UI.Inventory.Items;
using UnityEngine;

public class CollectCardSetsQuestStep : QuestStep {
    private int setsCollected = 0;
    public int setsToComplete = 3;

    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemCollectedItem += ItemCollected;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemCollectedItem -= ItemCollected;
    }

    private void ItemCollected(Item item) {
        if (item is not CollectibleCardSetItem)
            return;
        
        if (setsCollected < setsToComplete) {
            setsCollected++;
            UpdateState();
        }

        if (setsCollected >= setsToComplete) {
            UpdateState();
            FinishQuestStep();
        }
    }
    
    private void UpdateState() {
        string state = setsCollected.ToString();
        Debug.Log($"State: {state}");
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        setsCollected = int.Parse(state);
        UpdateState();
    }

    protected override void InitializeQuestStepState() {
        UpdateState();
    }
}
