using System.Collections;
using System.Collections.Generic;
using CardBattles.Particles;
using NaughtyAttributes;
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

        public void ForceLoad(SceneName sceneName) {
            Debug.Log(sceneName);
            var sceneString = GetSceneName(sceneName);
            ParticleParent.killAllParticles?.Invoke();
            SceneManager.LoadScene(sceneString);
        }
        public IEnumerator LoadSceneAsync(SceneName sceneName) {
            var sceneString = GetSceneName(sceneName);
            var lastProgress = 0f;
            operation = SceneManager.LoadSceneAsync(sceneString);

            LoadingScreen(true);
            operation.allowSceneActivation = false;

            while (!operation.isDone) {
                if (lastProgress - operation.progress >= 0.1f) {
                    lastProgress = operation.progress;
                    Debug.Log(lastProgress);
                }
                if (operation.progress >= 0.9f) {
                    LoadingScreen(false);

                    operation.allowSceneActivation = true;
                    ParticleParent.killAllParticles?.Invoke();
                }

                yield return null;
            }
        }

        private void LoadingScreen(bool val) {
            return;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadCardBattles() {
            StartCoroutine(LoadSceneAsync(SceneName.CardBattle));
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadOverworld() {
            StartCoroutine(LoadSceneAsync(SceneName.Overworld));
        }
    }
}