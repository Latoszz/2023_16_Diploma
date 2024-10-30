using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardBattles.CardGamesManager {
    public enum SceneName {
        CardBattle,
        Overworld
    }

    public class SceneLoader : MonoBehaviour {
        public static SceneLoader Instance;
        private AsyncOperation operation;


        [SerializeField] private string battleSceneName = "CardBattleScene";
        [SerializeField] private string overworldSceneName = "Overworld1";

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }

        private string GetSceneName(SceneName sceneName) {
            switch (sceneName) {
                case SceneName.CardBattle:
                    return battleSceneName;
                case SceneName.Overworld :
                    return overworldSceneName;
                default:
                    return null;
            }
        }

        public IEnumerator LoadSceneAsync(SceneName sceneName) {
            var sceneString = GetSceneName(sceneName);
            
            operation = SceneManager.LoadSceneAsync(sceneString);

            LoadingScreen(true);
            operation.allowSceneActivation = false;

            while (!operation.isDone) {
                if (operation.progress >= 0.9f) {
                    LoadingScreen(false);

                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private void LoadingScreen(bool val) {
            Debug.Log("implement loading screens");
            return;
        }
    }
}