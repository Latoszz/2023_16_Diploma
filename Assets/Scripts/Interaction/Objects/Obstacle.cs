using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction.Objects {
    public class Obstacle: MonoBehaviour {
        [SerializeField] private string obstacleID;
        [ContextMenu("Generate guid for id")]
        [Button]
        private void GenerateGuid() {
            obstacleID = Guid.NewGuid().ToString();
        }

        public void SetObstacle(bool value) {
            gameObject.SetActive(value);
        }

        public bool IsObstacle() {
            return gameObject.activeSelf;
        }

        public string GetID() {
            return obstacleID;
        }

        public static UnityEvent<string> getObtacleEvent;
        
    }
}