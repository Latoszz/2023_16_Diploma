using CardBattles.CardGamesManager;
using EnemyScripts;
using InputScripts;
using SaveSystem;
using UI.Inventory;
using UnityEngine;

namespace Interaction.EnemyInteractions {
    public class EnemyPopup : MonoBehaviour {
        [SerializeField] private GameObject enemyPopup;
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private GameObject deckPopup;

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
            if (CheckDeck()) {
                Close();
                InventoryDeckManager.Instance.UpdateDeck();
                SaveManager.Instance.SaveGame();
                CardGamesLoader.Instance.BeginBattle(EnemyStateManager.Instance.GetCurrentEnemy().GetBattleData());
            }
            else {
                deckPopup.SetActive(true);
            }
        }

        public void NoClicked() {
            popupPanel.SetActive(false);
            Close();
            InventoryController.Instance.SetBattle(false);
        }

        public void ClosePopup() {
            deckPopup.SetActive(false);
            Close();
            InventoryController.Instance.ShowInventory();
            InventoryController.Instance.SetBattle(true);
        }

        private void Close() {
            enemyPopup.SetActive(false);
            InputManager.Instance.EnableAllInput();
        }

        private bool CheckDeck() {
            int occupiedCount = 0;
            foreach (ItemSlot slot in InventoryController.Instance.GetDeckSlots()) {
                if (slot.IsOccupied())
                    occupiedCount++;
            }
            return occupiedCount == InventoryController.Instance.GetDeckSlots().Count;
        }
    }
}