using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class obstacleDataManager : MonoBehaviour {
    public static obstacleDataManager Instance;
    [SerializeField] private tmpListObstacle tmpListObstacle;
    private static int i = -1;
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void changeNextObstacle(bool val) {
        if (val)
            i += 1;
        aa();
    }

    public void aa() {
        for (int j = 0; j < tmpListObstacle.obstacles.Count; j++) {
            tmpListObstacle.obstacles[j].SetObstacle( j<=i);
        }
    }
    
}
