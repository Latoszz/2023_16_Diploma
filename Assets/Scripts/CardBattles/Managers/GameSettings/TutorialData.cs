using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using UnityEngine;

namespace CardBattles.Managers.GameSettings {
    [CreateAssetMenu(fileName = "Tutorial Data", menuName = "Cards/Tutorial Data", order = 1)]
    public class TutorialData : ScriptableObject {
        [Header("Player Cards")]
        [SerializeField]
        public List<CardData> playerCards;

        [Header("Enemy Cards")]
        [SerializeField]
        public List<CardData> enemyCards;

        [Header("Enemy Actions")]
        [SerializeField]
        public List<EnemyAiAction> enemyActions;
    }
}