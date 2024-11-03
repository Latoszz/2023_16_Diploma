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

using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuestSystem {
    public class QuestPoint: MonoBehaviour, IPointerClickHandler {
        [Header("Quest")]
        [SerializeField] private QuestInfoSO questInfo;

        [Header("Config")] 
        [SerializeField] private bool startPoint = false;
        [SerializeField] private bool finishPoint = false;
        
        private string questId;
        private QuestState currentQuestState;

        private void Awake() {
            questId = questInfo.id;
        }

        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        }
        
        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if(currentQuestState.Equals(QuestState.CAN_START) && startPoint)
                GameEventsManager.Instance.QuestEvents.StartQuest(questId);
            else if(currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
                GameEventsManager.Instance.QuestEvents.FinishQuest(questId);
        }

        private void QuestStateChange(Quest quest) {
            if (quest.info.id.Equals(questId)) {
                currentQuestState = quest.state;
            }
        }
    }
}