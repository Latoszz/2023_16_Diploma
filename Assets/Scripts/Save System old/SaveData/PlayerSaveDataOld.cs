public class PlayerSaveDataOld : SaveDataOld {
    [System.Serializable]
    public struct PlayerData {
        public float[] position;
    }

    public PlayerData playerData;
}