using System.Collections;
using EnemyScripts;
using Events;
using InputScripts;
using QuestSystem;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NPCScripts {
    public class NPCBattleTrigger : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private QuestInfoSO questInfo;
        [Range(0, 10)] [SerializeField] private float detectionDistance;
        [SerializeField] private GameObject enemyPanel;

        private GameObject player;
        private Enemy enemy;

        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange += Unlock;
        }
        
        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= Unlock;
        }

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
            enemy = GetComponent<Enemy>();
        }

        private void Unlock(Quest quest) {
            if (quest.info.id == questInfo.id && quest.state == QuestState.CAN_FINISH) {
                enemy.ChangeState(EnemyState.Undefeated);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (enemy.GetState() != EnemyState.Undefeated) return;
            
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                StartCoroutine(StartBattleCoroutine());
            }
        }

        private IEnumerator StartBattleCoroutine() {
            yield return new WaitUntil(() => DialogueController.Instance.DialogueClosed);
            enemyPanel.SetActive(true);
            enemyPanel.transform.GetChild(0).gameObject.SetActive(true);
            EnemyStateManager.Instance.SetCurrentEnemy(enemy);
            InputManager.Instance.DisableAllInput();
        }
    }
}
