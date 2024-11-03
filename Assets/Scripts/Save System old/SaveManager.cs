using System.IO;
using UnityEngine;

namespace Save_System_old {
    public static class SaveManager {
        private static SaveSystemOld saveSystem = new JSONSaveSystem();

        public static string settingsSavePath = "/settings.json";
        public static string inventorySavePath = "/inventory.json";
        public static string npcSavePath = "/npc.json";
        public static string enemySavePath = "/enemy.json";
    
        public static bool saveExists;
        public static bool settingsSaveExists => File.Exists(Application.persistentDataPath + settingsSavePath);
        public static bool inventorySaveExists => File.Exists(Application.persistentDataPath + inventorySavePath);
        public static bool npcSaveExists => File.Exists(Application.persistentDataPath + npcSavePath);
        public static bool enemySaveExists => File.Exists(Application.persistentDataPath + enemySavePath);
    
        public static bool SaveGame<T>(string relativePath, T data) {
            return saveSystem.SaveData(relativePath, data);
        }

        public static T LoadGame<T>(string relativePath) {
            return saveSystem.LoadData<T>(relativePath);
        }
    }
}
