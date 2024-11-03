using System;
using UnityEngine;

namespace Items {
    [Serializable]
    [CreateAssetMenu(fileName = "New Collectible Item Data", menuName = "Items/Item Data")]
    public class CollectibleItemData: ScriptableObject {
        public string itemID;
    }
}