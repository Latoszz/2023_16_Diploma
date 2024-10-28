using System;

namespace Events {
    public class ObstacleEvents {
        public event Action<bool> OnObstacleChange;
        
        public void ObstacleChange(bool value) {
            OnObstacleChange?.Invoke(value);
        }
    }
}