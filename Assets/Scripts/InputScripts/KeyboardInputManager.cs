using UI.Infos;
using UI.Inventory;
using UI.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputScripts {
    [DefaultExecutionOrder(-90)]
    public class KeyboardInputManager : MonoBehaviour, PlayerControls.IPlayerActionMapActions {

        private PlayerControls playerControls;

        private void Awake() {
            playerControls = new PlayerControls();
        }
    
        private void Start() {
            playerControls.PlayerActionMap.SetCallbacks(this);
            playerControls.PlayerActionMap.Enable();
        }
    
        public void OnInventory(InputAction.CallbackContext context) {
            if (!context.performed) return;
            if (PauseManager.Instance.IsOpen)
                return;
        
            InventoryController inventoryController = InventoryController.Instance;
            if (inventoryController.IsOpen()) {
                inventoryController.HideInventory();
                playerControls.PlayerActionMap.Pause.Enable();
            }
            else {
                inventoryController.ShowInventory();
                playerControls.PlayerActionMap.Pause.Disable();
            }
        }
    
        public void OnPause(InputAction.CallbackContext context) {
            if (!context.performed) 
                return;
        
            if(InventoryController.Instance.IsOpen() || QuestListPanel.Instance.IsOpen)
                return;
        
            if (PauseManager.Instance.IsOpen) {
                PauseManager.Instance.Close();
                playerControls.PlayerActionMap.Inventory.Enable();
            }
            else {
                PauseManager.Instance.Open();
                playerControls.PlayerActionMap.Inventory.Disable();
            }
        }

        public void OnEscape(InputAction.CallbackContext context) {
            if (!context.performed) 
                return;
            
            InventoryController inventoryController = InventoryController.Instance;
            QuestListPanel questListPanel = QuestListPanel.Instance;
            
            if (!inventoryController.IsOpen() && !questListPanel.IsOpen) 
                return;
        
            if (inventoryController.IsCardSetDetailsOpen()) {
                inventoryController.HideCardSetDetails();
            }

            if (questListPanel.IsOpen) {
                QuestListPanel.Instance.Close();
            }
        }

        public void OnQuest(InputAction.CallbackContext context) {
            QuestListPanel.Instance.OpenClosePanel();
        }
    }
}