using System.Collections.Generic;
using System.IO;
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
            string[] files = Directory.GetFiles(path, "*.asset", SearchOption.TopDirectoryOnly);

            foreach (var file in files) {
                var asset = AssetDatabase.LoadAssetAtPath<BattleData>(file);
                if (asset is not BattleData cardBattle) continue;
                if (battleDatasWithFlags.ContainsKey(asset)) continue;
                i++;
                Debug.Log($"Added {asset.name}, to list of battles");
                battleDatasWithFlags.Add(asset, BattleState.NotPlayed);
            }
            Debug.Log($"Added {i}, new battles");

        }
#endif
    }
}