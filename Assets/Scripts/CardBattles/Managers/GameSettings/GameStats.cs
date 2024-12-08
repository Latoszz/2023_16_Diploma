using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CardBattles.Managers.GameSettings {
    public static class GameStats
    {
        private static GameBalanceConfig config;

        public static GameBalanceConfig Config
        {
            get
            {
#if UNITY_EDITOR
                if (config == null)
                {
                    Debug.Log("Config loaded from resources");
                    config = Resources.Load<GameBalanceConfig>("GameBalanceConfig");
                    if (config == null)
                    {
                        Debug.LogError("GameBalanceConfig asset not found in Resources folder!");
                    }
                }
                return config;
#else
                Debug.LogWarning("Access to GameBalanceConfig is disabled in builds.");
                return new GameBalanceConfigWrapper(); // Returns a wrapper object with all properties defaulted to false.
#endif
            }
        }
        // ReSharper disable InconsistentNaming
        private static bool _isTutorial = false;
        public static bool isTutorial {
            // ReSharper restore InconsistentNaming
    
            get {
                #if UNITY_EDITOR
                return Config.isTutorial;
                #else
                return _isTutorial;
                #endif
            }
            set {
                _isTutorial = value;
            }
        }
    
        public static TutorialData CurrentTutorialData => Config.isTutorial ? Config.tutorialData : null;

    }

    // Wrapper class to return default values in builds
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GameBalanceConfigWrapper : GameBalanceConfig
    {
#pragma warning disable CS0108, CS0114
        public  bool cardsExtraSleep => false;
        public  bool overrideHeroMaxHp => false;
        public  int overrideHeroMaxHpValue => 0;
        public bool isTutorial => false;
        public TutorialData tutorialData => null;
        public bool resetDeckWhenEmpty => false;
#pragma warning restore CS0108, CS0114

    }
}