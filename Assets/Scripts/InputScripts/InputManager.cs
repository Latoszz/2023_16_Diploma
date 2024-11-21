using UnityEngine;

namespace InputScripts {
    [DefaultExecutionOrder(-100)] // so that the input manager loads before keyboard and mouse managers 
    public class InputManager : MonoBehaviour {
        public static InputManager Instance = null;

        [SerializeField] private MouseInputManager mouseInputManager;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            //DontDestroyOnLoad(this);
        }

        public void EnableInput() {
            mouseInputManager.EnableMouseControls();
        }

        public void DisableInput() {
            mouseInputManager.DisableMouseControls();
        }
    }
}