using System;
using UI.Dialogue;

namespace Events {
    public class DialogueEvents {
        public event Action<string, DialogueText> OnDialogueEnded;
        
        public void DialogueEnded(string name, DialogueText dialogueText) {
            OnDialogueEnded?.Invoke(name, dialogueText);
        }
    }
}