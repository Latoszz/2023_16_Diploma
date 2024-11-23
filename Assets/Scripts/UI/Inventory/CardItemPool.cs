using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {
    public class CardItemPool : MonoBehaviour {
        [SerializeField] private GameObject cardItemPrefab;
        [SerializeField] private int initialPoolSize = 5;
    
        private List<GameObject> pool = new List<GameObject>();

        private void Awake() {
            for (int i = 0; i < initialPoolSize; i++) {
                GameObject item = Instantiate(cardItemPrefab, transform);
                item.SetActive(false);
                pool.Add(item);
            }
        }
    
        public GameObject GetCardItem() {
            foreach (var item in pool) {
                if (!item.activeInHierarchy) {
                    item.SetActive(true);
                    return item;
                }
            }
        
            GameObject newItem = Instantiate(cardItemPrefab, transform, false);
            pool.Add(newItem);
            return newItem;
        }
    
        public void ReturnCardItem(GameObject item) {
            item.SetActive(false);
        }
    }
}