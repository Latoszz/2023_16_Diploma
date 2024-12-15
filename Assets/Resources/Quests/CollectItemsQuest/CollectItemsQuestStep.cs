using System;
using Events;
using QuestSystem;

public class CollectItemsQuestStep : QuestStep {
    private int itemsCollected = 0;
    public int itemsToComplete = 3;

    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemCollected += ItemCollected;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemCollected -= ItemCollected;
    }

    private void ItemCollected() {
        if (itemsCollected < itemsToComplete) {
            itemsCollected++;
            UpdateState();
        }

        if (itemsCollected >= itemsToComplete) {
            UpdateState();
            FinishQuestStep();
        }
    }
    
    private void UpdateState() {
        string state = itemsCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        itemsCollected = Convert.ToInt32(state);
        UpdateState();
    }
    
    protected override void InitializeQuestStepState() {
        UpdateState();
    }
}
