using InputScripts;
using SaveSystem;
using UI.HUD;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace UI.Menu {
    public class PauseManager : MonoBehaviour {
        [SerializeField] private GameObject pauseView;
        [SerializeField] private GameObject optionsView;
        [SerializeField] private GameObject infoPanel;
    
        private PostProcessVolume postProcessVolume;
    
        public bool IsOpen { get; set; }

        public static PauseManager Instance = null;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            
            pauseView.SetActive(false);
            postProcessVolume = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
        }

        public void Open() {
            if (InventoryController.Instance.IsOpen())
                return;
            pauseView.SetActive(true);
            optionsView.SetActive(false);
            infoPanel.SetActive(false);
            postProcessVolume.enabled = true;
            IsOpen = true;
            HUDController.Instance.HideHUD();
            InputManager.Instance.DisableInput();
            Time.timeScale = 0;
        }

        public void Close() {
            pauseView.SetActive(false);
            optionsView.SetActive(false);
            infoPanel.SetActive(false);
            postProcessVolume.enabled = false;
            IsOpen = false;
            HUDController.Instance.ShowHUD();
            InputManager.Instance.EnableInput();
            Time.timeScale = 1;
        }

        public void ContinueClicked() {
            Close();
        }

        public void SaveClicked() {
            //TODO save
        }

        public void OptionsClicked() {
            pauseView.SetActive(false);
            optionsView.SetActive(true);
        }

        public void ExitClicked() {
            infoPanel.SetActive(true);
        }

        public void Exit() {
            Close();
            SaveManager.Instance.SaveGame();
            SceneManager.LoadScene("Main Menu");
        }

        public void BackClicked() {
            pauseView.SetActive(true);
            optionsView.SetActive(false);
        }
    }
}
