using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InputScripts {
    [DefaultExecutionOrder(-100)] // so that the input manager loads before keyboard and mouse managers 
    public class InputManager : MonoBehaviour {
        public static InputManager Instance = null;

        [SerializeField] private MouseInputManager mouseInputManager;
        
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
        
        public void EnableInput() {
            mouseInputManager.EnableMouseControls();
        }

        public void DisableInput() {
            mouseInputManager.DisableMouseControls();
        }
    }
}