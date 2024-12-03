using System;

namespace Events {
    public class TutorialEvents {
        public event Action OnActivateClickPoint;
        
        public void ActivateClickPoint() {
            OnActivateClickPoint?.Invoke();
        }
    }
}