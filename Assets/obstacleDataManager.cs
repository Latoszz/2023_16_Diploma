using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SaveSystem;
using UnityEngine;
using UnityEngine.Events;

public class obstacleDataManager : MonoBehaviour {
    public static obstacleDataManager Instance;
    [SerializeField] private tmpListObstacle tmpListObstacle;
    [SerializeField] [HideInInspector]
    private int i = -1;

    [ShowNativeProperty]
    private int I => i;
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void changeNextObstacle(bool val) {
        if (val) {
            i += 1;
            SaveManager.Instance.ChangeObstacleData(tmpListObstacle.obstacleIds[i], false);
        }

        Debug.Log("i: " +i);
        aa();
    }

    public void aa() {
        Debug.Log("i: " +i);
        for (int j = 0; j < tmpListObstacle.obstacles.Count; j++) {
            if (tmpListObstacle.obstacles[j] is not null) {
                var tmpObstacle = tmpListObstacle.obstacles[j];
                var boolean = (j > i);

                Debug.Log($"{tmpObstacle.GetID()}: {tmpObstacle.IsObstacle()}=> {boolean}");
                //tmpObstacle.SetObstacle(boolean);
            }
        }
    }
    
}
