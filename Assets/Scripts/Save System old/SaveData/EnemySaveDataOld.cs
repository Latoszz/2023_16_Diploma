using System.Collections.Generic;

public class EnemySaveDataOld : SaveDataOld {
    [System.Serializable]
    public struct EnemyData {
        public int state;
        public float[] position;
        public bool isActive;
    }
    
    public List<EnemyData> enemyDatas = new List<EnemyData>();
}