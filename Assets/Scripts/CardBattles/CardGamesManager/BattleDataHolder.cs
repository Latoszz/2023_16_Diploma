using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace CardBattles.CardGamesManager {
    public class BattleDataHolder : MonoBehaviour {
        [SerializeField] private BattleDataFlagDictionary battleDatasWithFlags;
        public static BattleDataHolder Instance;

        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }

        public BattleData DebugGetFirstBattleData() {
            return battleDatasWithFlags.Keys.First();
        }
        public BattleState GetBattleDataState(BattleData battleData) {
            return battleDatasWithFlags[battleData];
        }
        public void SetBattleDataState(BattleData battleData, bool val) {
            var battleState = val ? BattleState.Beaten : BattleState.Played;

            Debug.Log($"{battleDatasWithFlags[battleData]}  =>  {battleState}"); 
            battleDatasWithFlags[battleData] = battleState;
        }
        
#if UNITY_EDITOR
        [Button]
        public void LoadCardBattleData() {
            var i = 0;
            var path = "Assets/Scripts/CardBattles/Scriptable objects/CardBattle";
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);

            foreach (var asset in assets) {
                if (asset is not BattleData cardBattle) continue;
                if (battleDatasWithFlags.ContainsKey((BattleData)asset)) continue;
                i++;
                Debug.Log($"Added {asset.name}, to list of battles");
                battleDatasWithFlags.Add((BattleData)asset, false);
            }
            Debug.Log($"Added {i}, new battles");

        }
#endif
    }
}