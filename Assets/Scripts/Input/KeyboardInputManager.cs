using UnityEngine;
using UnityEngine.InputSystem;

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
        if (!inventoryController.IsOpen()) 
            return;
        
        if (inventoryController.IsCardSetDetailsOpen()) {
            inventoryController.HideCardSetDetails();
        }
    }
}