using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnemyScripts;
using Esper.ESave;
using NaughtyAttributes;
using QuestSystem;
using SaveSystem.SaveData;
using UI.Menu;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0414
namespace SaveSystem {
    public class SaveManager: MonoBehaviour {
        [SerializeField] private SaveFileSetup saveFileSetup;
        [SerializeField] private string MainMenuSceneName = "Main Menu";
        [SerializeField] private string TutorialSceneName = "TutorialOverworld";
        [SerializeField] private string RoomSceneName = "Room under the statue";
        [SerializeField] private string OverworldSceneName = "Overworld1";
        [SerializeField] private string CardBattleSceneName = "CardBattle";
        
        private SaveFile saveFile;
        private List<ISavable> savableObjects;

        private const string InitialSaveDataID = "Initial save data";
        private const string InventorySaveDataID = "Inventory items";
        private const string TutorialSaveData = "Tutorial";

        public static SaveManager Instance;

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.name == CardBattleSceneName) {
                //LoadSettings();
                return;
            }
            
            if (scene.name == TutorialSceneName) {
                LoadSettings();
                return;
            }
            
            if (scene.name == RoomSceneName) {
                LoadInventory();
                LoadSettings();
                QuestManager.Instance.LoadQuests(saveFile);
                return;
            }
            
            LoadSaveFile();
        }
        
        public void LoadSaveFile() {
            saveFile = saveFileSetup.GetSaveFile();
            savableObjects = FindAllSavableObjects();
            LoadGame();
        }

        private void Awake() {
            if (Instance != null) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void NewGame() {
            DeleteSaveFile();
            saveFile.AddOrUpdateData(InitialSaveDataID, 0);
            saveFile.AddOrUpdateData(TutorialSaveData, true);
            SettingsManager.Instance.PopulateSaveData(saveFile);
            saveFile.Save();
        }

        public void SaveGame() {
            if (!HasSaveData()) {
                Debug.Log("Tried saving but no data");
                return;
            }
            
            foreach (ISavable savableObject in savableObjects) {
                savableObject.PopulateSaveData(saveFile);
            }
            QuestManager.Instance.SaveQuests(saveFile);
            saveFile.Save();
            Debug.Log("Game saved");
        }

        public void LoadGame() {
            if (!HasSaveData()) {
                Debug.Log("Tried loading but no data");
                return;
            }
            
            foreach (ISavable savableObject in savableObjects) {
                savableObject.LoadSaveData(saveFile);
            }
            QuestManager.Instance.LoadQuests(saveFile);
        }
        

        private List<ISavable> FindAllSavableObjects() {
            IEnumerable<ISavable> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ISavable>();
            return new List<ISavable>(objects);
        }

        private void LoadSettings() {
            SettingsManager settingsManager = FindObjectOfType<SettingsManager>(true);
            settingsManager.LoadSaveData(saveFile);
        }

        public void SaveSettings() {
            SettingsManager settingsManager = FindObjectOfType<SettingsManager>(true);
            settingsManager.PopulateSaveData(saveFile);
            saveFile.Save();
        }

        private void LoadInventory() {
            InventoryDataHandling inventoryDataHandling = FindObjectOfType<InventoryDataHandling>(true);
            inventoryDataHandling.LoadSaveData(saveFile);
        }

        public void SaveInventory() {
            InventoryDataHandling inventoryDataHandling = FindObjectOfType<InventoryDataHandling>(true);
            inventoryDataHandling.PopulateSaveData(saveFile);
            saveFile.Save();
        }
        
        private void LoadQuests() {
            QuestManager questManager = FindObjectOfType<QuestManager>(true);
            questManager.LoadQuests(saveFile);
        }

        public void SaveQuests() {
            QuestManager questManager = FindObjectOfType<QuestManager>(true);
            questManager.SaveQuests(saveFile);
        }

        private bool HasSaveData() {
            return saveFile.HasData(InitialSaveDataID);
        }

        public bool HasInventoryData() {
            return saveFile.HasData(InventorySaveDataID);
        }

        public bool IsAfterTutorial() {
            if (!saveFile.HasData(TutorialSaveData))
                return false;
            return !saveFile.GetData<bool>(TutorialSaveData);
        }

        public void ChangeTutorialData(bool value) {
            saveFile.AddOrUpdateData(TutorialSaveData, value);
            saveFile.Save();
        }

        public void ChangeObstacleData(string dataId, bool value) {
            saveFile.AddOrUpdateData(dataId, value);
            saveFile.Save();
        }

        public void ChangeEnemyData(string enemyId, EnemyState enemyState) {
            saveFile.AddOrUpdateData(enemyId, enemyState);
            saveFile.Save();
        }

        private void OnApplicationQuit() {
            SaveGame();
        }
        
        private void DeleteSaveFile() {
            saveFile.EmptyFile();
            string path = GetFilePath();
            File.Delete(path);
            Debug.LogWarning($"Save file deleted at path: {path}");
        }
        
        
        #if UNITY_EDITOR
        [Button]
        public void OpenSaveFile() {
            string path = GetFilePath();
            EditorUtility.RevealInFinder(path);
        }
        
        [Button]
        public void DeleteSaveFileEditor() {
            string path = GetFilePath();
            FileUtil.DeleteFileOrDirectory(path);
            AssetDatabase.Refresh();
            Debug.LogWarning($"Save file deleted at path: {path}");
        }
        #endif

        private string GetFilePath() {
            string mainPath = saveFileSetup.saveFileData.saveLocation switch {
                SaveFileSetupData.SaveLocation.PersistentDataPath => Application.persistentDataPath,
                SaveFileSetupData.SaveLocation.DataPath => Application.dataPath,
                _ => ""
            };
            string filePath = saveFileSetup.saveFileData.filePath;
            string fileName = saveFileSetup.saveFileData.fileName;
            string fileExtension = saveFileSetup.saveFileData.fileType.ToString().ToLower();
            string path = Path.Combine(mainPath, filePath, fileName + "." + fileExtension);
            return path;
        }
    }
}