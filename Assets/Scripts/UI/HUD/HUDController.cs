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
            InventoryController.Instance.ShowInventory();
        }

        public void HideHUD() {
            overlay.SetActive(false);
        }

        public void ShowHUD() {
            overlay.SetActive(true);
        }
    }
}