using System;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;

namespace CardBattles.CardGamesManager {
    [CreateAssetMenu(fileName = "BattleData", menuName = "Cards", order = 0)]
    [Serializable]
    public class BattleData : ScriptableObject {
        [SerializeField] private List<CardSetData> enemyCardSets;
        public List<CardSetData> GetCardSets => enemyCardSets;
        //we can add special cases here
    }
}