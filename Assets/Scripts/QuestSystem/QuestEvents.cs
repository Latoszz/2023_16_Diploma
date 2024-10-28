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

using System;
namespace QuestSystem {
    public class QuestEvents {
        public event Action<string> OnStartQuest;
        public void StartQuest(string id) {
            OnStartQuest?.Invoke(id);
        }
        
        public event Action<string> OnAdvanceQuest;
        public void AdvanceQuest(string id) {
            OnAdvanceQuest?.Invoke(id);
        }
        
        public event Action<string> OnFinishQuest;
        public void FinishQuest(string id) {
            OnFinishQuest?.Invoke(id);
        }
        
        public event Action<Quest> OnQuestStateChange;
        public void QuestStateChange(Quest quest) {
            OnQuestStateChange?.Invoke(quest);
        }
        
        public event Action<string, int, QuestStepState> OnQuestStepStateChange;
        public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState) {
            OnQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
        }
    }
}