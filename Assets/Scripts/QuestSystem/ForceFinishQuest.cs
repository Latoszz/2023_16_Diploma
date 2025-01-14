using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuestSystem {
    public class ForceFinishQuest: MonoBehaviour, IPointerClickHandler {
        [SerializeField] private QuestInfoSO questInfoSo;
        [Range(0, 10)] [SerializeField] private int detectionDistance;
        private GameObject player;
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!(Vector3.Distance(player.transform.position, gameObject.transform.position) <= detectionDistance)) return;
            GameEventsManager.Instance.QuestEvents.FinishQuest(questInfoSo.id);
        }
    }
}