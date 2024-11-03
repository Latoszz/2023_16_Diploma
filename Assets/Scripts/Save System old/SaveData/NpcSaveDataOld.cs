using System.Collections.Generic;

public class NpcSaveDataOld : SaveDataOld{
    [System.Serializable]
    public struct NPCData {
        public float[] position;
        public bool isActive;
    }
    
    public List<NPCData> npcDatas = new List<NPCData>();
}