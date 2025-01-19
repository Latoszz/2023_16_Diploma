using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Character;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using UnityEngine;

public class Secrets : PlayerEnemyMonoBehaviour {
    [SerializeField] private CardSetData cardSetData;
    private CharacterManager character;
    private bool hasDone = false;
    private void Awake() {
        character = GetComponent<CharacterManager>();
    }
    
    private void AddCard() {
        if(hasDone) return;
        if(!TurnManager.Instance.isPlayersTurn) return;
        if(!character.IsPlayers) return;
        
        var cardData = cardSetData.cards.First();
        character.deck.AddCard(cardData);
        CharacterManager.DrawACard(character,0);
        hasDone = true;
    }
    
    [SerializeField] private KeyCode toggleKey = KeyCode.L; 

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            AddCard();
        }
    }
}
