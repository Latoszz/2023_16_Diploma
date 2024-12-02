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

        [Header("Deck")]
        public bool resetDeckWhenEmpty = false;

        [SerializeField] [ResizableTextArea, ReadOnly]
        private string textureGuideLines = "Card textures should be 32 x 32\nicons should be 8 x8";


    }
}