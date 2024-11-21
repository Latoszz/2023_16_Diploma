using System;

namespace Events {
    public class DialogueEvents {
        public event Action<string> OnDialogueEnded;
        
        public void DialogueEnded(string name) {
            OnDialogueEnded?.Invoke(name);
        }
    }
}