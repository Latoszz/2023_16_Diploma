using Events;
using QuestSystem;

public class CollectCardSetsQuestStep : QuestStep {
    private int setsCollected = 0;
    public int setsToComplete = 3;
    public string cardSetName;

    private void OnEnable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected += ItemCollected;
    }

    private void OnDisable() {
        GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected -= ItemCollected;
    }

    private void ItemCollected(string itemName) {
        if (itemName != cardSetName)
            return;
        
        if (setsCollected < setsToComplete) {
            setsCollected++;
        }

        if (setsCollected >= setsToComplete) {
            FinishQuestStep();
        }
    }


    protected override void SetQuestStepState(string state) {
        setsCollected = int.Parse(state);
    }
}
