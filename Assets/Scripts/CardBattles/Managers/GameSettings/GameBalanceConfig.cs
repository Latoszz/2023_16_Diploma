using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.Managers.GameSettings {
    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Cards/Game Balance Config", order = 1)]
    public class GameBalanceConfig : ScriptableObject {
        
        [Header("Slow down game")][SerializeField]
        public bool cardsExtraSleep = false;
    
        [Header("Hero Settings")][SerializeField]
        public bool overrideHeroMaxHp = false;
        public int overrideHeroMaxHpValue = 15;

        [Header("Deck")][SerializeField]
        public bool resetDeckWhenEmpty = false;

        [SerializeField] [ResizableTextArea, ReadOnly]
        private string textureGuideLines = "Card textures should be 32 x 32\n" +
                                           "";


        [Header("Tutorial")][SerializeField]
        public bool isTutorial = false; //refer to GameStats.isTutorial, this only does anything in editor
        [SerializeField]
        public TutorialData tutorialData;


        
    }
}