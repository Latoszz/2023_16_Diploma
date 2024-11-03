using System;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject {
    [SerializeField] private string enemyName;
    [SerializeField] private List<CardSetData> deck = new List<CardSetData>();

    public List<CardSetData> GetDeck() {
        return deck;
    }
}
