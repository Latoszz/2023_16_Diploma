using Esper.ESave;
using Esper.ESave.SavableObjects;
using SaveSystem;
using UnityEngine;
using UnityEngine.AI;

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
            SavableTransform savableTransform = saveFile.GetTransform(PlayerSaveID);
            transform.CopyTransformValues(savableTransform);
        }
    }
}
