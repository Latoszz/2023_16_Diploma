using System.Collections.Generic;
using System.Linq;
using Esper.ESave;
using Interaction.Objects;
using Items;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class LevelDataHandling:MonoBehaviour, ISavable {
        private List<ICollectible> allItems;
        private List<Obstacle> allObstacles;

        private const string ItemSaveData = "Collectible item data";
        private const string ObstacleSaveData = "Obstacle data";

        private void Awake() { 
            IEnumerable<Obstacle> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<Obstacle>();
            allObstacles = new List<Obstacle>(objects);
            
            IEnumerable<ICollectible> items = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ICollectible>();
            allItems = new List<ICollectible>(items);
        }
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(ObstacleSaveData, ObstacleSaveData);
            foreach (Obstacle obstacle in allObstacles) {
                string id = obstacle.GetID();
                if(saveFile.HasData(id))
                    saveFile.DeleteData(id);
                saveFile.AddOrUpdateData(id, obstacle.IsObstacle());
            }
            
            saveFile.AddOrUpdateData(ItemSaveData, ItemSaveData);
            foreach (ICollectible item in allItems) {
                MonoBehaviour itemMonoBehaviour = (MonoBehaviour)item;
                if (itemMonoBehaviour is CollectibleItem collectibleItem) {
                    string id = collectibleItem.GetID();
                    if(saveFile.HasData(id))
                        saveFile.DeleteData(id);
                    saveFile.AddOrUpdateData(id, collectibleItem.IsCollected());
                }
                else {
                    CollectibleCardSetItem cardSetItem = (CollectibleCardSetItem)itemMonoBehaviour;
                    string id = cardSetItem.GetID();
                    if(saveFile.HasData(id))
                        saveFile.DeleteData(id);
                    saveFile.AddOrUpdateData(id, cardSetItem.IsCollected());
                }
            }
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (!saveFile.HasData(ObstacleSaveData))
                return;
            
            foreach (Obstacle obstacle in allObstacles) {
                bool isObstacle = saveFile.GetData<bool>(obstacle.GetID());
                obstacle.SetObstacle(isObstacle);
            }
            
            if (!saveFile.HasData(ItemSaveData))
                return;
            
            foreach (ICollectible item in allItems) {
                MonoBehaviour itemMonoBehaviour = (MonoBehaviour)item;
                if (itemMonoBehaviour is CollectibleItem collectibleItem) {
                    bool isCollected = saveFile.GetData<bool>(collectibleItem.GetID());
                    collectibleItem.SetCollected(isCollected);
                }
                else {
                    CollectibleCardSetItem cardSetItem = (CollectibleCardSetItem)itemMonoBehaviour;
                    bool isCollected = saveFile.GetData<bool>(cardSetItem.GetID());
                    cardSetItem.SetCollected(isCollected);
                }
            }
        }
    }
}