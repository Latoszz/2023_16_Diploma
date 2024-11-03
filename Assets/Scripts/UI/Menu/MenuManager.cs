#if UNITY_EDITOR
using UnityEditor;
#endif
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    [SerializeField] private GameObject mainView;
    [SerializeField] private GameObject optionsView;
    [SerializeField] private GameObject creditsView;
    [SerializeField] private Button continueButton;

    [SerializeField] private GameObject audioVideoPanel;
    [SerializeField] private GameObject controlsPanel;
    
    private void Awake() {
        BackClicked();
    }

    private void Start() {
        if(SaveManager.Instance.HasSaveData())
            continueButton.interactable = true;
    }

    #region  Main View
    public void StartClicked(string sceneName) {
        SaveManager.Instance.NewGame();
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueClicked(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OptionsClicked() {
        mainView.SetActive(false);
        optionsView.SetActive(true);
        audioVideoPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ControlsClicked() {
        audioVideoPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void CreditsClicked() {
        mainView.SetActive(false);
        creditsView.SetActive(true);
    }

    public void ExitClicked() {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    #endregion

    public void BackClicked() {
        mainView.SetActive(true);
        optionsView.SetActive(false);
        creditsView.SetActive(false);
    }
}
