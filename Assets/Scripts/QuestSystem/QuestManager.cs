/***
 * MIT License

Copyright (c) 2023 Shaped by Rain Studios

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 ***/

using System.Collections.Generic;
using Esper.ESave;
using Events;
using UI.Inventory;
using UI.Inventory.Items;
using UnityEngine;

namespace QuestSystem {
    public class QuestManager: MonoBehaviour {
        private Dictionary<string, Quest> questsDict;

        public static QuestManager Instance;
        private static string QuestSaveID = "Quest data";

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            questsDict = CreateQuestsDict();
        }

        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += StartQuest;
            GameEventsManager.Instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestStepStateChange += QuestStepStateChange;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest += FinishQuest;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= StartQuest;
            GameEventsManager.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestStepStateChange -= QuestStepStateChange;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;
        }

        private void Start() {
            foreach (Quest quest in questsDict.Values) {
                if (quest.state == QuestState.IN_PROGRESS) {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
            }
        }

        private void Update() {
            foreach (Quest quest in questsDict.Values) {
                if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)) {
                    ChangeQuestState(quest.info.id, QuestState.CAN_START);
                }
            }
        }

        private void ChangeQuestState(string id, QuestState state) {
            Quest quest = GetQuestById(id);
            quest.state = state;
            GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
        }

        private bool CheckRequirementsMet(Quest quest) {
            bool meetsRequirements = true;
            foreach (QuestInfoSO questPrerequisite in quest.info.questPrerequisites) {
                if (GetQuestById(questPrerequisite.id).state != QuestState.CAN_FINISH) { //WAS checking if FINISHED, might change it back
                    meetsRequirements = false;
                    break;
                }
            }
            return meetsRequirements;
        }

        private void StartQuest(string id) {
            Quest quest = GetQuestById(id);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        }
        
        private void AdvanceQuest(string id) {
            Quest quest = GetQuestById(id);
            quest.MoveToNextStep();
            if (quest.CurrentStepExists()) {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            else {
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
            }
        }
        
        private void FinishQuest(string id) {
            Quest quest = GetQuestById(id);
            ClaimRewards(quest);
            ChangeQuestState(quest.info.id, QuestState.FINISHED);
        }

        private void ClaimRewards(Quest quest) {
            Item[] rewards = quest.info.questRewards;
            foreach (Item reward in rewards) {
                InventoryController.Instance.AddItem(reward);
                GameEventsManager.Instance.ItemEvents.ItemReward(reward.GetName());
            }
        }

        private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState) {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.state);
        }

        public void ForceQuestFinish(string id) {
            FinishQuest(id);
        }
        
        private Dictionary<string, Quest> CreateQuestsDict() {
            QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
            Dictionary<string, Quest> idToQuestDict = new Dictionary<string, Quest>();
            
            foreach (QuestInfoSO questInfo in allQuests) {
                idToQuestDict.Add(questInfo.id, new Quest(questInfo));
            }
            
            return idToQuestDict;
        }

        public Quest GetQuestById(string id) {
            return questsDict[id];
        }

        public Dictionary<string, Quest> GetQuestList() {
            return questsDict;
        }

        public void SaveQuests(SaveFile saveFile) {
            saveFile.AddOrUpdateData(QuestSaveID, QuestSaveID);
            foreach (Quest quest in questsDict.Values) {
                string id = quest.info.id;
                if(saveFile.HasData(id))
                    saveFile.DeleteData(id);

                string questStepStates = JsonUtility.ToJson(quest.GetQuestStepStates());
                saveFile.AddOrUpdateData(id + "_state", quest.state);
                saveFile.AddOrUpdateData(id + "_stepIndex", quest.GetCurrentQuestStepIndex());
                saveFile.AddOrUpdateData(id + "_stepStates", questStepStates);
                saveFile.Save();
            }
        }

        public void LoadQuests(SaveFile saveFile) {
            if (!saveFile.HasData(QuestSaveID))
                return;
            
            foreach (Quest quest in questsDict.Values) {
                string id = quest.info.id;
                QuestState state = saveFile.GetData<QuestState>(id + "_state");
                int currentQuestStepIndex = saveFile.GetData<int>(id + "_stepIndex");
                string questStepStates = saveFile.GetData<string>(id + "_stepStates");
                quest.SetCurrentQuestStepIndex(currentQuestStepIndex);
                ChangeQuestState(id, state);
            }
        }
    }
}