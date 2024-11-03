using Esper.ESave;

namespace SaveSystem {
    public interface ISavable {
        void PopulateSaveData(SaveFile saveFile);
        void LoadSaveData(SaveFile saveFile);
    }
}