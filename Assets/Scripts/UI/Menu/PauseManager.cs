using InputScripts;
using UI.HUD;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using SaveManager = SaveSystem.SaveManager;

namespace UI.Menu {
    public class PauseManager : MonoBehaviour {
        [SerializeField] private GameObject pauseView;
        [SerializeField] private GameObject optionsView;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject infoPopup;
        [SerializeField] private GameObject questPanel;
    
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
            infoPopup.SetActive(false);
            questPanel.SetActive(false);
            postProcessVolume.enabled = true;
            IsOpen = true;
            HUDController.Instance.HideHUD();
            InputManager.Instance.DisableAllInput();
            Time.timeScale = 0;
        }

        public void Close() {
            pauseView.SetActive(false);
            optionsView.SetActive(false);
            infoPanel.SetActive(false);
            infoPopup.SetActive(true);
            questPanel.SetActive(true);
            postProcessVolume.enabled = false;
            IsOpen = false;
            HUDController.Instance.ShowHUD();
            InputManager.Instance.EnableAllInput();
            Time.timeScale = 1;
        }

        public void ContinueClicked() {
            Close();
        }

        public void SaveClicked() {
            SaveManager.Instance.SaveGame();
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
            infoPanel.SetActive(false);
        }
    }
}
