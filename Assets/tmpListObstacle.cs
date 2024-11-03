using System.Collections;
using System.Collections.Generic;
using Interaction.Objects;
using NaughtyAttributes;
using UnityEngine;

public class tmpListObstacle : MonoBehaviour {
    [OnValueChanged("UpdateObstacleIdsCallback")]
    [SerializeField] public List<Obstacle> obstacles;
    [SerializeField] public List<string> obstacleIds;

    [Button]
    void UpdateObstacleIdsCallback() {
        obstacleIds = new List<string>();
        foreach (var VARIABLE in obstacles) {
            obstacleIds.Add("");
        }
        for (int i = 0; i < obstacles.Count; i++) {
           
            if (obstacles[i] is not null) {
                obstacleIds[i] = obstacles[i].GetID();
            }
        }
    }
    
  
}
