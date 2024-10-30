using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CardBattles.CardGamesManager {
    [DefaultExecutionOrder(-1)]
    public class CardGamesLoader : MonoBehaviour {
        public static CardGamesLoader Instance;
        public BattleData currentBattleData = null;


        [SerializeField] public UnityEvent<List<CardSetData>> loadEnemyCards;
        [SerializeField] public UnityEvent<List<CardSetData>> loadPlayerCards;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }


        public void BeginBattle(BattleData battleData, List<CardSetData> playerCardSetDatas) {
            currentBattleData = battleData;
            StartCoroutine(SceneLoader.Instance.LoadSceneAsync(SceneName.CardBattle));
            loadEnemyCards?.Invoke(battleData.GetCardSets);
            loadPlayerCards?.Invoke(playerCardSetDatas);
        }

        public void EndGame(bool val) {
            BattleDataHolder.Instance.SetBattleDataState(currentBattleData, val);
            currentBattleData = null;
            StartCoroutine(SceneLoader.Instance.LoadSceneAsync(SceneName.Overworld));
        }
    }
}