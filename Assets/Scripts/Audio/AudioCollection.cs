    using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Audio {
    public class AudioCollection : MonoBehaviour
    {

        private string json ;
   
        private string jsonFilePath;
        private string jsonBackupPath;
    
        public static AudioCollection Instance;
    
        [InfoBox(text: "Steps:\n" +
                       "1. Add .GetClip(string) to code\n" +
                       "2. Use the script (playmode or editmode)\n" +
                       "3. Exit playmode\n" +
                       "4. Press 'Load Data From Json'\n" +
                       "5. Add Audioclips (first move them to Resources/Audio/)\n" +
                       "6, Press 'Update Json From Dictionary")]
        public List<AudioEntry> audioMap = new List<AudioEntry>();

        private void Awake() {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(this);
        }


        void Start()
        {
            #if UNITY_EDITOR
            LoadDataFromJson();
            #endif
        }

    
        public void CreateJsonPaths() {
            jsonFilePath = "Assets/Resources/Audio/AudioJson/" +"audioMap.json";
            string todayString = DateTime.Now.ToString("yyyy-MM-dd");
            string backupAudioFileName = "backupAudio_" + todayString + ".json";
            jsonBackupPath = "Assets/Resources/Audio/AudioJson/Backup/" + backupAudioFileName;
            //Debug.Log(jsonFilePath);
            //Debug.Log(jsonBackupPath);
        }
    
        [Button]
        private void LoadDataFromJson() {
            CreateJsonPaths();
            GetJsonFileData();
            var result = FlattenJson(JObject.Parse(json));
            var noAudioMessage = "No audio for: \n";
            bool everyKeyHasAudio = true;

            audioMap.Clear(); 
            foreach (var entry in result) {
                AudioClip clip = Resources.Load<AudioClip>($"Audio/{entry.Value}");
                audioMap.Add(new AudioEntry { key = entry.Key, clip = clip });
                if (String.IsNullOrEmpty(entry.Value)) {
                    {
                        noAudioMessage += $"{entry.Key}, ";
                        everyKeyHasAudio = false;
                        continue;
                    }
                }

                if (clip is null)
                {
                    everyKeyHasAudio = false;
                    Debug.LogWarning($"AudioClip '{entry.Value}' could not be found for key '{entry.Key}'");
                }
            }
            if(everyKeyHasAudio)
                return;
            print(noAudioMessage);
        }
        private void GetJsonFileData()
        {
            if (File.Exists(jsonFilePath))
            {
                try
                {
                    json = File.ReadAllText(jsonFilePath);
                    Debug.Log($"Loaded JSON from {jsonFilePath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load JSON from file: {e.Message}");
                }
            }
            else
            {
                Debug.Log("No saved JSON file found; using default JSON.");
            }
        }
        static Dictionary<string, string> FlattenJson(JObject json, string parentKey = "")
        {
            var result = new Dictionary<string, string>();

            foreach (var property in json.Properties())
            {
                string currentKey = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";

                if (property.Value is JObject childObject)
                {
                    var childResult = FlattenJson(childObject, currentKey);
                    foreach (var child in childResult)
                    {
                        result.Add(child.Key, child.Value);
                    }
                }
                else
                {
                    result[currentKey] = property.Value.ToString();
                }
            }

            return result;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public AudioClip GetClip(string key)
        {
            // Try to find the audio entry by the key
            var entry = audioMap.Find(e => e.key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (entry != null)
            {
                return entry.clip;
            }
            
            #if UNITY_EDITOR
            // If the entry is not found, add a new one with a null AudioClip
            var newEntry = new AudioEntry { key = key, clip = null };
            audioMap.Add(newEntry);
            Debug.Log($"Added new key '{key}' to the audio map with a null AudioClip. You can assign a clip later.");
            UpdateJsonFromDictionary();
            #endif
            return null;
        }
#if UNITY_EDITOR
        // ReSharper disable Unity.PerformanceAnalysis
        [Button]
        private void UpdateJsonFromDictionary()
        {      
            CreateJsonPaths();

            var nestedDictionary = new Dictionary<string, object>();

            foreach (var entry in audioMap)
            {
                AddToNestedDictionary(nestedDictionary, entry.key.Split('.'), entry.clip ? entry.clip.name : "");
            }

            json = JsonConvert.SerializeObject(nestedDictionary, Formatting.Indented);
            Debug.Log("JSON overwritten with current audio map:\n" + json);
        
            SaveJsonToFile();
        }
    
        private void AddToNestedDictionary(Dictionary<string, object> dict, string[] keys, string value)
        {
            var currentDict = dict;
            if (currentDict is null) {
                Debug.LogWarning("THIS SHOULD NOT BE NULL");
                return;
            }
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!currentDict.ContainsKey(keys[i]))
                {
                    currentDict[keys[i]] = new Dictionary<string, object>();
                }

                currentDict = currentDict[keys[i]] as Dictionary<string, object>;
            }

            currentDict[keys[keys.Length - 1]] = value;
        }
    
   
        private void SaveJsonToFile()
        {
            try {
                if (!File.Exists(jsonFilePath))
                    File.Create(jsonFilePath);
                File.WriteAllText(jsonFilePath, json);
                AssetDatabase.SaveAssets();
                Debug.Log($"JSON saved to {jsonFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save JSON to file: {e.Message}");
            }
        }
        [Button]
        public void SaveJsonToBackupFile()
        {
            CreateJsonPaths();

            try {
                LoadDataFromJson();
                if (!File.Exists(jsonBackupPath))
                    File.Create(jsonBackupPath);
                File.WriteAllText(jsonBackupPath, json);
                AssetDatabase.SaveAssets();
                Debug.Log($"JSON saved to {jsonBackupPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save JSON to file: {e.Message}");
            }
        }
#endif
    }
}
