using System.Collections.Generic;
using CardBattles.Enums;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace CardBattles.CardScripts.CardDatas {
    public abstract class CardData : ScriptableObject {
        [SerializeField] [HideInInspector]
        public CardSetData cardSet;

        [SerializeField]
        public string cardName;

        [SerializeField, ShowAssetPreview]
        public Sprite sprite;

        [TextArea] [SerializeField]
        public string description;

        [TextArea] [SerializeField]
        public string flavourText;

        [SerializeField]
        public List<AdditionalProperty> properties = new List<AdditionalProperty>();

        [SerializeField]
        private TriggerEffectDictionary effectDictionary = new TriggerEffectDictionary();

        public TriggerEffectDictionary EffectDictionary {
            get {
                var copyEffectDictionary = new TriggerEffectDictionary();
                foreach (var effectTrigger in effectDictionary.Keys) {
                    copyEffectDictionary.Add(effectTrigger, effectDictionary[effectTrigger].Copy());
                }

                return copyEffectDictionary;
            }

            set => effectDictionary = value;
        }
    }
}