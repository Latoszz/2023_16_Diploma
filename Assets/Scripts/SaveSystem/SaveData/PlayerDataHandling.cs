using Esper.ESave;
using Esper.ESave.SavableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace SaveSystem.SaveData {
    public class PlayerDataHandling : MonoBehaviour, ISavable {
        private const string PlayerSaveID = "Player transform";

        private NavMeshAgent navMeshAgent;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(PlayerSaveID, transform);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (saveFile.HasData(PlayerSaveID)) {
                navMeshAgent.ResetPath();
                navMeshAgent.enabled = false;
                SavableTransform savableTransform = saveFile.GetTransform(PlayerSaveID);
                transform.CopyTransformValues(savableTransform);
                navMeshAgent.enabled = true;
            }
        }
    }
}
