using System.Collections.Generic;
using InputScripts;
using UI.Infos;
using UI.Inventory;
using UI.Menu;
using UnityEngine;

namespace UI.HUD {
    public class HUDController : MonoBehaviour {
        [SerializeField] private List<GameObject> overlay;
        public static HUDController Instance;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }
    
        public void PauseGame() {
            PauseManager.Instance.Open();
        }

        public void OpenInventory() {
            if (QuestListPanel.Instance.IsOpen)
                QuestListPanel.Instance.OpenClosePanel();
            InventoryController.Instance.ShowInventory();
            InputManager.Instance.DisablePause();
        }

        public void HideHUD() {
            foreach(GameObject o in overlay)
                o.SetActive(false);
        }

        public void ShowHUD() {
            foreach(GameObject o in overlay)
                o.SetActive(true);
        }
    }
}