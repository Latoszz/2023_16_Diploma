using System;

namespace Events {
    public class TutorialEvents {
        public event Action OnActivateClickPoint;
        
        public void ActivateClickPoint() {
            OnActivateClickPoint?.Invoke();
        }
        
        public event Action OnUnlockStatue;

        public void UnlockStatue() {
            OnUnlockStatue?.Invoke();
        }
        
        public event Action OnUnlockInventory;

        public void UnlockInventory() {
            OnUnlockInventory?.Invoke();
        }
    }
}