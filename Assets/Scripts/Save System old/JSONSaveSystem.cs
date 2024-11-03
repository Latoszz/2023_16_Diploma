using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Save_System_old {
    public class JSONSaveSystem : SaveSystemOld {

        public override bool SaveData<T>(string relativePath, T data) {
            string path = Application.persistentDataPath + relativePath;

            try {
                if(File.Exists(path))
                    File.Delete(path);
                FileStream stream = File.Create(path);
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch (Exception e) {
                Debug.LogError("Unable to save data due to " + e.Message + " " + e.StackTrace);
                return false;
            }

        }

        public override T LoadData<T>(string relativePath) {
            string path = Application.persistentDataPath + relativePath;
            if (!File.Exists(path)) {
                throw new FileNotFoundException();
            }

            try {
                T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                return data;
            }
            catch (Exception e) {
                Debug.LogError("Unable to load data due to " + e.Message + " " + e.StackTrace);
                throw;
            }
        }
    }
}
