using NPC;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPopupTrigger : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private GameObject enemyPanel;
    [SerializeField] private TalkableNPC npc;
    public void OnPointerClick(PointerEventData eventData) {
        if (PauseManager.Instance.IsOpen)
            return;
        EnemySM enemy = gameObject.GetComponent<EnemySM>();
        if (npc.TalkedTo()) {
            enemy.ChangeState(EnemyState.Undefeated);
        }
        
        if (enemy.GetState() == EnemyState.Undefeated) {
            enemyPanel.SetActive(true);
            enemyPanel.transform.GetChild(0).gameObject.SetActive(true);
            EnemyPopup.Instance.Enemy = enemy;
            InputManager.Instance.DisableInput();
        }
    }
}
