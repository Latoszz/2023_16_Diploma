
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneTriggerClick : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private string loadName;
    [SerializeField] private GameObject popupPanel;
    [Range(0, 10f)]
    [SerializeField] private float detectionDistance;

    private GameObject player;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
            popupPanel.SetActive(true);
            InputManager.Instance.DisableInput();
        }
    }

    public void YesClicked() {
        popupPanel.gameObject.SetActive(false);
        InputManager.Instance.EnableInput();
        SceneSwitcher.Instance.UnloadScene(SceneManager.GetActiveScene().name);
        SceneSwitcher.Instance.LoadScene(loadName);
    }

    public void NoClicked() {
        popupPanel.gameObject.SetActive(false);
        InputManager.Instance.EnableInput();
    }
    
}