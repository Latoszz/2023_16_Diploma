using System;
using CardBattles.CardScripts.CardDatas;
using UnityEditor;
using UnityEngine;

namespace Editor.CardBattles {
    [CustomEditor(typeof(CardData), true)]
    public class CardDataEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUI.changed && target != null) {
                EditorUtility.SetDirty(target);
                var cardData = (CardData)target;
                if (cardData != null) {
                    var nameGiven = cardData.cardName;
                    if (string.IsNullOrWhiteSpace(nameGiven)) {
                        nameGiven = "No name given";
                    }
                    cardData.name = nameGiven;
                    AssetDatabase.SaveAssets();
                }
            }
        }

    }
}