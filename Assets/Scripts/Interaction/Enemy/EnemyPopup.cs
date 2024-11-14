using SaveSystem;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyPopup : MonoBehaviour {
    [SerializeField] private GameObject enemyPopup;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject deckPopup;
    [SerializeField] private string BattleSceneName = "CardBattleScene";

    public EnemySM Enemy;
    public bool IsOpen => enemyPopup.activeSelf;

    public static EnemyPopup Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
    
    public void YesClicked() {
        popupPanel.SetActive(false);
        if (CheckDeck(3)) {
            Close();
            InventoryDeckManager.Instance.UpdateDeck();
            SaveManager.Instance.SaveGame();
            SceneSwitcher.Instance.LoadScene(BattleSceneName);
        }
        else {
            deckPopup.SetActive(true);
        }
    }

    public void NoClicked() {
        popupPanel.SetActive(false);
        Close();
    }

    public void ClosePopup() {
        deckPopup.SetActive(false);
        Close();
        InventoryController.Instance.ShowInventory();
    }

    private void Close() {
        Enemy = null;
        enemyPopup.SetActive(false);
        InputManager.Instance.EnableInput();
    }

    private bool CheckDeck(int count) {
        int occupiedCount = 0;
        foreach (ItemSlot slot in InventoryController.Instance.GetDeckSlots()) {
            if (slot.IsOccupied())
                occupiedCount++;
        }
        return occupiedCount == count;
    }
}