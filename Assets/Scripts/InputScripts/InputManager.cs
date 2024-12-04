using UI.Infos;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InputScripts {
    [DefaultExecutionOrder(-100)] // so that the input manager loads before keyboard and mouse managers 
    public class InputManager : MonoBehaviour {
        public static InputManager Instance = null;

        [SerializeField] private MouseInputManager mouseInputManager;
        [SerializeField] private KeyboardInputManager keyboardInputManager;
        
        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            //DontDestroyOnLoad(this);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // Reset the input system
            Mouse.current?.MakeCurrent();
            InputSystem.ResetHaptics();
            InputSystem.Update(); 
        }
        
        public void EnableAllInput() {
            EnableMoveInput();
            EnableInventory();
            EnableQuestPanel();
        }

        public void DisableAllInput() {
            DisableMoveInput();
            DisableInventory();
            DisableQuestPanel();
        }
        
        public void EnableMoveInput() {
            mouseInputManager.EnableMouseControls();
        }

        public void DisableMoveInput() {
            mouseInputManager.DisableMouseControls();
        }

        public void EnableInventory() {
            mouseInputManager.EnableInventory();
            keyboardInputManager.EnableInventory();
        }

        public void DisableInventory() {
            mouseInputManager.DisableInventory();
            keyboardInputManager.DisableInventory();
        }
        
        public void EnableQuestPanel() {
            keyboardInputManager.EnableQuests();
        }

        public void DisableQuestPanel() {
            keyboardInputManager.DisableQuests();
        }
    }
}