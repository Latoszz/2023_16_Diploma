using System.Collections.Generic;
using System.Linq;
using EnemyScripts;
using Esper.ESave;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class EnemyDataHandling: MonoBehaviour, ISavable {
        private List<Enemy> allEnemies;
        private const string EnemySaveData = "Enemy data";

        private void Awake() {
            IEnumerable<Enemy> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<Enemy>();
            allEnemies = new List<Enemy>(objects);
        }
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(EnemySaveData, EnemySaveData);
            foreach (Enemy enemy in allEnemies) {
                string id = enemy.GetID();
                if(saveFile.HasData(id))
                    saveFile.DeleteData(id);

                saveFile.AddOrUpdateData(id, enemy.GetState());
            }
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (!saveFile.HasData(EnemySaveData))
                return;
            
            foreach (Enemy enemy in allEnemies) {
                EnemyState state = saveFile.GetData<EnemyState>(enemy.GetID());
                enemy.ChangeState(state);
            }
        }
    }
}