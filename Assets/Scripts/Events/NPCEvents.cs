using System;

namespace Events {
    public class NPCEvents {
        public event Action<string> OnTalkedToNPC;

        public void TalkedToNPC(string talkableNpc) {
            OnTalkedToNPC?.Invoke(talkableNpc);
        }
    }
}