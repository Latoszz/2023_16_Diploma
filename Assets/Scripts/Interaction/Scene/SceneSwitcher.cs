using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static SceneSwitcher Instance = null;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPivot;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject); 
    }

    public void LoadScene(string sceneName) {
        player.SetActive(sceneName is "beta-release" or "beta-release-2");
        if(sceneName is "beta-release-2")
            player.transform.position = new Vector3(28, 6.7f, -4);
        //cameraPivot.SetActive(sceneName != "Irys playspace");
        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ExitOutOfBattles(bool gameWon) {
        //TODO this is called from endgamemanager
        //Trigger flag that this battle is done
        //Send some signal to something that changes when flag is true
        //Exit out of battle
    }
    
    public void UnloadScene(string sceneName) {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }
}
