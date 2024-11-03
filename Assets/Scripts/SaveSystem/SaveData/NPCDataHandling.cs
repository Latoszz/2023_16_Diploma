using System.Collections.Generic;
using System.Linq;
using Esper.ESave;
using NPC;
using UnityEngine;

namespace SaveSystem.SaveData {
    public class NPCDataHandling: MonoBehaviour, ISavable {
        private List<TalkableNPC> allNPCs;
        private const string NPCSaveData = "NPC save data";

        private void Awake() {
            IEnumerable<TalkableNPC> objects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<TalkableNPC>();
            allNPCs = new List<TalkableNPC>(objects);
        }
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(NPCSaveData, NPCSaveData);
            foreach (TalkableNPC npc in allNPCs) {
                string id = npc.GetID();
                if(saveFile.HasData(id))
                    saveFile.DeleteData(id);
                
                int j = npc.dialogue.Count();
                saveFile.AddOrUpdateData(id, j);
            }
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (!saveFile.HasData(NPCSaveData))
                return;
            
            foreach (TalkableNPC npc in allNPCs) {
                int j = npc.dialogue.Count;
                int dialogueIndex = saveFile.GetData<int>(npc.GetID());
                while (j != dialogueIndex) {
                    npc.dialogue.Remove(npc.dialogue[0]);
                    j--;
                }
            }
        }
    }
}