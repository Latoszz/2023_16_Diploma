using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;
using UnityEngine;

namespace UI.Inventory {
    public class InventoryDeckManager: MonoBehaviour {
        public static InventoryDeckManager Instance;
        private List<CardSetData> deck;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }         
            else if (Instance != this) {
                Destroy(gameObject);
            }          
            DontDestroyOnLoad(gameObject); 
            
        }
        
        public void UpdateDeck() { 
            deck = InventoryController.Instance.GetDeck();
        }

        public List<CardSetData> GetDeck() {
            return deck;
        }

      
    }
}