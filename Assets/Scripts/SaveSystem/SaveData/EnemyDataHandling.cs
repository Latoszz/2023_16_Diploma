using System.Collections.Generic;
using System.Linq;
using Esper.ESave;
using NPC;
using ScriptableObjects.Dialogue;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class EnemyDataHandling: MonoBehaviour, ISavable {
        private List<EnemySM> allEnemies;
        private const string EnemySaveData = "Enemy data";

        private void Awake() {
            IEnumerable<EnemySM> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<EnemySM>();
            allEnemies = new List<EnemySM>(objects);
        }
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(EnemySaveData, EnemySaveData);
            foreach (EnemySM enemy in allEnemies) {
                string id = enemy.GetID();
                if(saveFile.HasData(id))
                    saveFile.DeleteData(id);

                saveFile.AddOrUpdateData(id, enemy.GetState());
            }
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (!saveFile.HasData(EnemySaveData))
                return;
            
            foreach (EnemySM enemy in allEnemies) {
                EnemyState state = (EnemyState)saveFile.GetData<int>(enemy.GetID());
                enemy.ChangeState(state);
            }
        }
    }
}