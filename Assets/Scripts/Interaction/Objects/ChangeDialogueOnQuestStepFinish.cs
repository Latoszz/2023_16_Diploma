using EnemyScripts;
using Events;
using NPCScripts;
using QuestSystem;
using UnityEngine;

namespace Interaction.Objects {
    public class ChangeDialogueOnQuestStepFinish : MonoBehaviour {
        [SerializeField] private QuestInfoSO quest;
        [SerializeField] private TalkableNPC talkableNpc;
        [SerializeField] private TalkableEnemy talkableEnemy;
        [SerializeField] private bool unlockEnemy;

        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange += ChangeDialogue;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= ChangeDialogue;
        }

        private void ChangeDialogue(Quest quest) {
            if (quest.info.id != this.quest.id) return;
            if (quest.state != QuestState.CAN_FINISH) return;
            if (talkableNpc is not null)
                talkableNpc.SetUpNextDialogue();
            else if (talkableEnemy is not null) {
                talkableEnemy.SetUpNextDialogue();
                if (unlockEnemy)
                    talkableEnemy.ChangeState(EnemyState.Undefeated);
            }
        }
    }
}
