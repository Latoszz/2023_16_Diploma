using NaughtyAttributes;
using SaveSystem;
using UnityEngine;

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
    }
    
}
