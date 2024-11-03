using QuestSystem;
using Items;
using UnityEngine;

namespace Events {
    public class GameEventsManager: MonoBehaviour {
        public static GameEventsManager Instance;

        [Header("Events")] 
        public QuestEvents QuestEvents;
        public ItemEvents ItemEvents;
        public ObstacleEvents ObstacleEvents;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            
            //initializing events
            QuestEvents = new QuestEvents();
            ItemEvents = new ItemEvents();
            ObstacleEvents = new ObstacleEvents();
        }
    }
}