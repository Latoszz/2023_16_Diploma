using System;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;

namespace CardBattles.CardGamesManager {
    [CreateAssetMenu(fileName = "New BattleData", menuName = "Cards/BattleData", order = 0)]
    [Serializable]
    public class BattleData : ScriptableObject {
        [SerializeField] private List<CardSetData> enemyCardSets;
        public List<CardSetData> GetCardSets => enemyCardSets;

        [SerializeField] public bool isTutorial = false;
        //we can add special cases here
    }
}