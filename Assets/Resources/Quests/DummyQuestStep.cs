using QuestSystem;

public class DummyQuestStep : QuestStep {
    private void Start() {
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state) {
        
    }
}
