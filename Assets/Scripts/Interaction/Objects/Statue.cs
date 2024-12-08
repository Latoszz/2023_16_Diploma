using Events;
using Interaction.Scene;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Statue : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private ShowOutline showOutlineScript;
        [SerializeField] private GameObject shakeCamera;
        [SerializeField] private string sceneName;

        private bool isActive;
        
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue -= Activate;
        }

        private void Activate() {
            showOutlineScript.enabled = true;
            shakeCamera.SetActive(true);
            isActive = true;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!isActive || shakeCamera.activeSelf)
                return;
            SaveManager.Instance.SaveSettings();
            SaveManager.Instance.SaveInventory();
            SceneSwitcher.Instance.LoadScene(sceneName);
        }
    }
}
