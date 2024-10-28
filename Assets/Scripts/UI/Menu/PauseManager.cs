using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseView;
    [SerializeField] private GameObject optionsView;
    [SerializeField] private GameObject audioVideoPanel;
    
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
        //DontDestroyOnLoad(this);
        pauseView.SetActive(false);
        postProcessVolume = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
    }

    public void Open() {
        pauseView.SetActive(true);
        optionsView.SetActive(false);
        postProcessVolume.enabled = true;
        IsOpen = true;
        HUDController.Instance.HideHUD();
        Time.timeScale = 0;
    }

    public void Close() {
        pauseView.SetActive(false);
        optionsView.SetActive(false);
        postProcessVolume.enabled = false;
        IsOpen = false;
        HUDController.Instance.ShowHUD();
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
        audioVideoPanel.SetActive(true);
    }

    public void ExitClicked() {
        Close();
        SceneSwitcher.Instance.LoadScene("Main Menu");
    }

    public void BackClicked() {
        pauseView.SetActive(true);
        optionsView.SetActive(false);
    }
}
