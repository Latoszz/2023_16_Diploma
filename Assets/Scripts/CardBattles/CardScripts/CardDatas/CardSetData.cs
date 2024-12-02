using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattles.CardScripts.CardDatas {
    [Serializable]
    [CreateAssetMenu(fileName = "New CardSet", menuName = "Cards/CardSet")]
    public class CardSetData :ScriptableObject {
        [SerializeField]
        public string displayName = "No Display Name";
        
        [HideInInspector]public List<CardData> cards = new List<CardData>();
        [SerializeField]
        public Color setColor = Color.white;
[ShowAssetPreview]
        [SerializeField] public Sprite cardSetIcon;
    }
}