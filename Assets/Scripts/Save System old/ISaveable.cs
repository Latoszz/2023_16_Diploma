public interface ISaveable {
    void PopulateSaveData(SaveDataOld saveDataOld);
    void LoadSaveData(SaveDataOld saveDataOld);
}