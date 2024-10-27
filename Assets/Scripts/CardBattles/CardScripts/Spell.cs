using System.Collections;
using System.Linq;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Enums;
using UnityEngine;

namespace CardBattles.CardScripts {
    public class Spell : Card {
        public override void Initialize(CardData cardData, bool isPlayersCard) {
            base.Initialize(cardData, isPlayersCard);
            if (cardData.EffectDictionary.Keys.Any(k => k != EffectTrigger.OnPlay)) {
                // Only the key EffectTrigger.OnPlay exists
                Debug.LogWarning($"SPELL {cardData.name} HAS EFFECTS IT SHOULD NOT HAVE, OR IT DOES NOT HAVE ONPLAY EFFECTS");
            }

            if (cardData is not SpellData spellData) 
                Debug.LogError("Invalid data type passed to Spell.Initialize");
            
            else 
                cardDisplay.SetCardDisplayData(spellData);
            
        }

        public override IEnumerator Play() {
            yield return StartCoroutine(base.Play());
            Destroy(this.gameObject);
        }
    }
}