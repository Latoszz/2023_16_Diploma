using UnityEngine;

[DefaultExecutionOrder(-100)] // so that the input manager loads before keyboard and mouse managers 
public class InputManager : MonoBehaviour {
    public static InputManager Instance = null;

    public PlayerControls playerControls;

    [SerializeField] private MouseInputManager mouseInputManager;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        //DontDestroyOnLoad(this);
        playerControls = new PlayerControls();
    }

    public void EnableInput() {
        mouseInputManager.EnableMouseControls();
    }

    public void DisableInput() {
        mouseInputManager.DisableMouseControls();
    }
}