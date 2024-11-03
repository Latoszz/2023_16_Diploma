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
        }

        if (itemsCollected >= itemsToComplete) {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state) {
        
    }
}
