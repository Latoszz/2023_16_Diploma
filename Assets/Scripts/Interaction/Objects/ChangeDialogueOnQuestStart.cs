using EnemyScripts;
using Events;
using NPCScripts;
using QuestSystem;
using UnityEngine;

namespace Interaction.Objects {
    public class ChangeDialogueOnQuestStart : MonoBehaviour {
        [SerializeField] private QuestInfoSO quest;
        [SerializeField] private TalkableNPC talkableNpc;
        [SerializeField] private TalkableEnemy talkableEnemy;
        [SerializeField] private bool unlockEnemy;

        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += ChangeDialogue;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= ChangeDialogue;
        }

        private void ChangeDialogue(string id) {
            if (id != quest.id) return;
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
