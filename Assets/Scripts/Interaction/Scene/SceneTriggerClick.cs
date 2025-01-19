using Events;
using InputScripts;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Interaction.Scene {
    public class SceneTriggerClick : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private string loadName;
        [SerializeField] private GameObject popupPanel;
        [Range(0, 10f)]
        [SerializeField] private float detectionDistance;

        private GameObject player;
        private bool goodEnding = false;

        private void OnEnable() {
            GameEventsManager.Instance.ItemEvents.OnItemCollected += ChangeEnding;
        }

        private void OnDisable() {
            GameEventsManager.Instance.ItemEvents.OnItemCollected -= ChangeEnding;
        }

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void ChangeEnding() {
            goodEnding = true;
        }
    
        public void OnPointerClick(PointerEventData eventData) {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance) {
                popupPanel.SetActive(true);
                InputManager.Instance.DisableAllInput();
            }
        }

        public void YesClicked() {
            popupPanel.gameObject.SetActive(false);
            InputManager.Instance.EnableAllInput();
            SaveManager.Instance.SaveSettings();
            SaveManager.Instance.SaveInventory();
            SaveManager.Instance.SaveQuests();
            SaveManager.Instance.ChangeTutorialData(false);
            SceneManager.LoadScene(loadName);
        }

        public void NoClicked() {
            popupPanel.gameObject.SetActive(false);
            InputManager.Instance.EnableAllInput();
        }
    
    }
}