using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardBattles.Managers.GameSettings {
    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Cards/Game Balance Config", order = 1)]
    public class GameBalanceConfig : ScriptableObject {
        
        [Header("Slow down game")]
        public bool cardsExtraSleep = false;
    
    
        [Header("Hero Settings")]
        public bool overrideHeroMaxHp = false;
        public int overrideHeroMaxHpValue = 15;

        
    
    
    }
}