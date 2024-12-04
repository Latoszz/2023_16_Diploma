using UI.Inventory;
using UnityEngine;

namespace UI.HUD {
    public class ButtonOutline : MonoBehaviour {
        [SerializeField] private GameObject outline;
        
        private void Update() {
            if (InventoryController.Instance.IsOpen()) {
                outline.SetActive(false);
            }
        }
    }
}
