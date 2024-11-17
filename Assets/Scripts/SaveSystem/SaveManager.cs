using System.Collections.Generic;
using System.IO;
using System.Linq;
using Esper.ESave;
using NaughtyAttributes;
using QuestSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem {
    public class SaveManager: MonoBehaviour {
        [SerializeField] private SaveFileSetup saveFileSetup;
        private SaveFile saveFile;
        private List<ISavable> savableObjects;

        private const string InitialSaveDataID = "Initial save data";
        private const string InventorySaveDataID = "Inventory items";

        public static SaveManager Instance;

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            Debug.Log("Scene loaded");
            if (scene.name != "Overworld1" && scene.name != "Main Menu") {
                return;
            }
            Debug.Log($"Scene {scene.name}");
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
                #if UNITY_EDITOR
                saveFile.AddOrUpdateData(InitialSaveDataID, 0);
                saveFile.Save();
                #endif
                return;
            }
            
            foreach (ISavable savableObject in savableObjects) {
                savableObject.LoadSaveData(saveFile);
            }
            QuestManager.Instance.LoadQuests(saveFile);
            Debug.Log("Game loaded");
        }
        

        private List<ISavable> FindAllSavableObjects() {
            IEnumerable<ISavable> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ISavable>();
            return new List<ISavable>(objects);
        }

        public bool HasSaveData() {
            return saveFile.HasData(InitialSaveDataID);
        }

        public bool HasInventoryData() {
            return saveFile.HasData(InventorySaveDataID);
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
        
#if UNITY_EDITOR
        [Button]
        public void OpenSaveFile() {
            string path = GetFilePath();
            EditorUtility.RevealInFinder(path);
        }
        
        [Button]
        public void DeleteSaveFile() {
            string path = GetFilePath();
            FileUtil.DeleteFileOrDirectory(path);
            AssetDatabase.Refresh();
            Debug.Log($"Save file deleted at path: {path}");
        }

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
#endif
    }
}