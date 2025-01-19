using Esper.ESave;
using Esper.ESave.SavableObjects;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class CameraDataHandling: MonoBehaviour, ISavable {
        private const string CameraSaveID = "Camera transform";
    
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(CameraSaveID, transform);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (saveFile.HasData(CameraSaveID)) {
                SavableTransform savableTransform = saveFile.GetTransform(CameraSaveID);
                transform.CopyTransformValues(savableTransform);
            }
        }
    }
}