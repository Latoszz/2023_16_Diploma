using System.Collections.Generic;
using Save_System_old;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour {
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EnemyStateManager enemyStateManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject npc2;
    [SerializeField] private EnemySM enemy;
    [SerializeField] private GameObject wall;
    [SerializeField] private ParticleSystem removablePS;
    [SerializeField] private List<CardSetItem> cardSets;
        
    public bool IsStarted => SceneManager.GetActiveScene().name is "beta-release" or "beta-release-2" or "New Scene";
    public static ManageGame Instance = null;
    
    public bool IsAfterTutorial { get; set; }
    public bool IsAfterSecondFight { get; set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == "Irys playspace")
            return;
        //LoadSettings();
        /*
        foreach (CardSetItem cardSet in cardSets) {
            InventoryController.Instance.AddItem(cardSet);
        }
        */
        
    }
    
    private void Update() {
        if (IsAfterTutorial && SceneManager.GetActiveScene().name == "beta-release") {
            enemy.ChangeState(EnemyState.Undefeated);
            IsAfterTutorial = false;
        }
        
    }
    public void SaveSettings() {
        SettingsSaveDataOld settingsSaveDataOld = new SettingsSaveDataOld();
        //settingsManager.PopulateSaveData(settingsSaveDataOld);
        SaveManager.SaveGame(SaveManager.settingsSavePath, settingsSaveDataOld);
    }

    public void LoadSettings() {
        if (SaveManager.settingsSaveExists) {
            SettingsSaveDataOld settingsSaveDataOld = SaveManager.LoadGame<SettingsSaveDataOld>(SaveManager.settingsSavePath);
            //settingsManager.LoadSaveData(settingsSaveDataOld);
        }
    }

    public void SaveInventory() {
        InventorySaveDataOld inventorySaveDataOld = new InventorySaveDataOld();
        inventoryController.PopulateSaveData(inventorySaveDataOld);
        SaveManager.SaveGame(SaveManager.inventorySavePath, inventorySaveDataOld);
    }

    public void LoadInventory() {
        if (SaveManager.inventorySaveExists) {
            InventorySaveDataOld inventorySaveDataOld = SaveManager.LoadGame<InventorySaveDataOld>(SaveManager.inventorySavePath);
            inventoryController.LoadSaveData(inventorySaveDataOld);
        }
    }
    
    public void SaveEnemy() {
        EnemySaveDataOld enemySaveDataOld = new EnemySaveDataOld();
        enemyStateManager.PopulateSaveData(enemySaveDataOld);
        SaveManager.SaveGame(SaveManager.enemySavePath, enemySaveDataOld);
    }
    
    public void LoadEnemy() {
        if (SaveManager.enemySaveExists) {
            EnemySaveDataOld enemySaveDataOld = SaveManager.LoadGame<EnemySaveDataOld>(SaveManager.enemySavePath);
            enemyStateManager.LoadSaveData(enemySaveDataOld);
        }
    }

}
