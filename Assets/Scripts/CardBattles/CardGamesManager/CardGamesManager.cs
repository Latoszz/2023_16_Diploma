using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CardBattles.CardGamesManager {
    public class CardGamesManager : MonoBehaviour {
        
        
        public static CardGamesManager Instance;
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }

        [SerializeField]
        public UnityEvent beginBattle;
        
        [SerializeField]
        private UnityEvent<List<CardSetData>> loadEnemyCards;
        [SerializeField]
        private UnityEvent<List<CardSetData>> loadPlayerCards;

        
        private void BeginBattle(BattleData battleData, List<CardSetData> playerCardSetDatas) {
            loadEnemyCards?.Invoke(battleData.GetCardSets);
            loadPlayerCards?.Invoke(playerCardSetDatas);
            
        }
        
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            //loadingScreen.SetActive(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            
            operation.allowSceneActivation = false;

            while (!operation.isDone) {
                // Rotate the symbol
                //rotatingSymbol.transform.Rotate(Vector3.forward * 100 * Time.deltaTime);

                // Check if the load has finished
                if (operation.progress >= 0.9f) {
                    // You can add some delay here if you want a smooth transition
                    operation.allowSceneActivation = true; // Activate the scene
                }

                yield return null;
            }

            // Hide the loading screen once loading is complete
            //loadingScreen.SetActive(false);
        }
   
    }
}
