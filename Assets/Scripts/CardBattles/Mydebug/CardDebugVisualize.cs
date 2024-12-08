using System.Collections.Generic;
using CardBattles.CardScripts;
using CardBattles.CardScripts.CardDatas;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardBattles.Mydebug {
    public class CardDebugVisualize : PlayerEnemyMonoBehaviour {
        private CardSetDictionary cardSets =
            new CardSetDictionary();

        private List<Card> cards = new List<Card>();

        [SerializeField] private CardCreator cardCreator;

        [FormerlySerializedAs("cardSetLoader")] [SerializeField]
        private CardSetLoaderEditor cardSetLoaderEditor;

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void KillAllChildren() {
            while (transform.childCount > 0) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void CreateCardSetsFromData() {
            int i = 0;
            if (cardSetLoaderEditor is null) {
                Debug.LogWarning("cardSetLoaderEditor is not found");
                return;
            }

            var cardSetDatas = cardSetLoaderEditor.cardSetDatas;
            foreach (var cardSetData in cardSetDatas) {
                i++;

                if (cardSetData == null) {
                    Debug.LogError("cardSetData is null at index ");
                    continue;
                }

                if (cardSetData.cards == null) {
                    Debug.LogError("cardSetData.cards is null for cardSetData: " + cardSetData.displayName);
                    continue;
                }

                var child = new GameObject(cardSetData.displayName);
                child.transform.SetParent(transform);

                var textObj = new GameObject("text");
                TextMeshPro tmpComponent = textObj.AddComponent<TextMeshPro>();
                textObj.transform.SetParent(child.transform);
                tmpComponent.alignment = TextAlignmentOptions.Right;

                /*
                var emptyObj = new GameObject("empty");
                var img = emptyObj.AddComponent<Image>();
                img.color = Color.clear;
                emptyObj.transform.SetParent(child.transform);*/

                HorizontalLayoutGroup hlGroup = child.AddComponent<HorizontalLayoutGroup>();
                hlGroup.spacing = 130;
                var lacksName = cardSetData.displayName == "No Display Name";
                tmpComponent.text = lacksName ? cardSetData.name : cardSetData.displayName;
                
                tmpComponent.text = tmpComponent.text.Replace("(Clone)", "").Trim();
                tmpComponent.enableWordWrapping = false;
                tmpComponent.fontSize = 300;
                tmpComponent.fontStyle = FontStyles.Bold;
                tmpComponent.color = Color.white;


                foreach (var cardData in cardSetData.cards) {
                    if (cardData == null) {
                        Debug.LogError("cardData is null in cardSetData: " + cardSetData.displayName);
                        continue;
                    }

                    var card = cardCreator.CreateCardInEditor(cardData, this);

                    if (card == null) {
                        Debug.LogError("Card creation failed for cardData in cardSetData: " + cardSetData.displayName);
                        continue;
                    }

                    card.transform.SetParent(child.transform, false);
                    card.ChangeCardVisible(true);

                    cardSets.TryAdd(cardSetData.displayName + i, new List<Card>());
                    var cardSetName = cardSetData.displayName + i;
                    cardSets[cardSetName].Add(card);
                    card.cardSetName = cardSetName;
                }
            }
        }
    }
}