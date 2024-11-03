using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IPointerClickHandler {
    public string sceneName;
    private const string ObstacleSaveID = "Obstacle ";
    
    public void OnPointerClick(PointerEventData eventData) {
        if(sceneName != "OverworldTesting")
            SaveManager.Instance.SaveGame();
        SaveManager.Instance.ChangeObstacleData(ObstacleSaveID + 0, false);
        SceneManager.LoadScene(sceneName);
    }
}
