using Events;
using InputScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Interaction.Scene {
    public class SceneTriggerClick : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private string loadName;
        [SerializeField] private GameObject popupPanel;
        [Range(0, 10f)]
        [SerializeField] private float detectionDistance;

        [SerializeField] private GameObject objectToCollect;

        private GameObject player;
        private bool goodEnding = false;

        private void OnEnable() {
            GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected += ChangeEnding;
        }

        private void OnDisable() {
            GameEventsManager.Instance.ItemEvents.OnItemWithIdCollected -= ChangeEnding;
        }

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void ChangeEnding(string itemName) {
            if (objectToCollect is null) return;
            if (itemName.Equals(objectToCollect.name)) {
                goodEnding = true;
            }
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
            SceneManager.LoadScene(loadName);
        }

        public void NoClicked() {
            popupPanel.gameObject.SetActive(false);
            InputManager.Instance.EnableAllInput();
        }
    
    }
}