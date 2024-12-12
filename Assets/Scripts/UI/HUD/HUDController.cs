using InputScripts;
using UI.Infos;
using UI.Inventory;
using UI.Menu;
using UnityEngine;

namespace UI.HUD {
    public class HUDController : MonoBehaviour {
        [SerializeField] private GameObject overlay;
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
            overlay.SetActive(false);
        }

        public void ShowHUD() {
            overlay.SetActive(true);
        }
    }
}