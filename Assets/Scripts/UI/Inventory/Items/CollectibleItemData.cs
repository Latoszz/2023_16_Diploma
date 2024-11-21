using System;
using UnityEngine;

namespace UI.Inventory.Items {
    [Serializable]
    [CreateAssetMenu(fileName = "New Collectible Item Data", menuName = "Items/Item Data")]
    public class CollectibleItemData: ScriptableObject {
        public string itemID;
    }
}