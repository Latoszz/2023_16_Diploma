using System;
using Events;
using QuestSystem;
using UnityEngine;

public class TalkToQuestStep : QuestStep {
    [SerializeField] private string npcName;
    private bool talkedTo;
    
    private void OnEnable() {
        GameEventsManager.Instance.NPCEvents.OnTalkedToNPC += TalkedTo;
    }

    private void OnDisable() {
        GameEventsManager.Instance.NPCEvents.OnTalkedToNPC -= TalkedTo;
    }

    private void TalkedTo(string npc) {
        if (npc == this.npcName) {
            UpdateState();
            FinishQuestStep(); 
        }
    }

    private void UpdateState() {
        string state = talkedTo.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        talkedTo = Convert.ToBoolean(state);
        UpdateState();
    }
}
