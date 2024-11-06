using System.Collections;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using NaughtyAttributes;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Events;


namespace CardBattles.CardGamesManager {
    [DefaultExecutionOrder(-1)]
    public class CardGamesLoader : MonoBehaviour {
        public static CardGamesLoader Instance;


        private BattleData currentBattleData = null;

        [ShowNativeProperty]
        private string CurrentBattleDataName {
            get {
                if (currentBattleData is null)
                    return "null";
                return currentBattleData.name;
            }
        }

        [SerializeField] [HideInInspector]public UnityEvent<List<CardSetData>> loadEnemyCards;
        [SerializeField] [HideInInspector]public UnityEvent<List<CardSetData>> loadPlayerCards;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }

        public void ForceBeginBattleDebug() {
            //currentBattleData = new BattleData();
            //InventoryDeckManager.Instance.GetDeck();
            SceneLoader.Instance.ForceLoad(SceneName.CardBattle);
            BattleDataHolder.Instance.DebugGetFirstBattleData();
        }

        public void BeginBattleDebug() {
            //currentBattleData = new BattleData();
            StartCoroutine(SceneLoader.Instance.LoadSceneAsync(SceneName.CardBattle));
            //InventoryDeckManager.Instance.GetDeck();
            BattleDataHolder.Instance.DebugGetFirstBattleData();
        }

        public IEnumerator BeginBattle(BattleData battleData, List<CardSetData> playerCardSetDatas = null) {
            playerCardSetDatas ??= InventoryDeckManager.Instance.GetDeck();
            
            currentBattleData = battleData;
            yield return StartCoroutine(
                SceneLoader.Instance.LoadSceneAsync(
                    SceneName.CardBattle));
            
            loadEnemyCards?.Invoke(battleData.GetCardSets);
            loadPlayerCards?.Invoke(playerCardSetDatas);
        }

        public void EndGame(bool val) {
            if (currentBattleData is not null)
                BattleDataHolder.Instance.SetBattleDataState(currentBattleData, val);
            /*
             * We can add a static event here, if you add a script like
             * class BattleDataContainer: MonoBehaviour
             * [serialize field] BattleData battledata
             *
             * który słucha i cos robi od razu jak zmieni się stan       
             */
            currentBattleData = null;
            StartCoroutine(SceneLoader.Instance.LoadSceneAsync(SceneName.Overworld));
        }
    }
}