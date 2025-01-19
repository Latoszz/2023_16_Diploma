using CardBattles;
using UnityEngine;

namespace UI {
    public class ResetButtonPosition: MonoBehaviour {
        private ButtonTextVal buttonScript;
        
        private void Awake() {
            buttonScript = GetComponent<ButtonTextVal>();
        }

        private void OnDisable() {
            buttonScript.ResetPosition();
        }
    }
}