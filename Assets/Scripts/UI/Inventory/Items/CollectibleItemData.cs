using System;
using UnityEngine;

namespace UI.Inventory.Items {
    [Serializable]
    [CreateAssetMenu(fileName = "New Collectible Item Data", menuName = "Items/Item Data")]
    public class CollectibleItemData: ScriptableObject {
        public Sprite itemSprite;
        public string itemID;
        [TextArea(5, 5)]
        public string text;
    }
}