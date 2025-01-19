using System;
using EnemyScripts;
using InputScripts;
using NPCScripts;
using UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.EnemyInteractions {
    [Obsolete]
    public class EnemyPopupTrigger : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject enemyPanel;
        [SerializeField] private TalkableNPC npc;
        public void OnPointerClick(PointerEventData eventData) {
            
            if (PauseManager.Instance.IsOpen)
                return;
            Enemy enemy = gameObject.GetComponent<Enemy>();
            if (npc.TalkedTo()) {
                enemy.ChangeState(EnemyState.Undefeated);
            }
            
        
            if (enemy.GetState() == EnemyState.Undefeated) {
                enemyPanel.SetActive(true);
                enemyPanel.transform.GetChild(0).gameObject.SetActive(true);
                EnemyStateManager.Instance.SetCurrentEnemy(enemy);
                InputManager.Instance.DisableAllInput();
            }
        }
    }
}
