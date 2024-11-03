
namespace Save_System_old {
    public abstract class SaveSystemOld {
        public abstract bool SaveData<T>(string relativePath, T data);

        public abstract T LoadData<T>(string relativePath);
    }
}
