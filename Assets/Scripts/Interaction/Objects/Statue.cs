using Events;
using Interaction.Scene;
using QuestSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Statue : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private ShowOutline showOutlineScript;
        [SerializeField] private GameObject shakeCamera;
        [SerializeField] private string sceneName;
        [SerializeField] private GameObject particleEffect;

        private bool isActive;
        private const int stepsToUnlock = 2;
        private int unlockedSteps = 0;
        
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue -= Activate;
        }

        private void Activate() {
            if (unlockedSteps < stepsToUnlock) {
                unlockedSteps++;
            }
            if(unlockedSteps >= stepsToUnlock) {
                showOutlineScript.enabled = true;
                shakeCamera.SetActive(true);
                isActive = true;
                particleEffect.SetActive(true);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!isActive || shakeCamera.activeSelf)
                return;
            SaveManager.Instance.SaveSettings();
            SaveManager.Instance.SaveInventory();
            SaveManager.Instance.SaveQuests();
            SceneSwitcher.Instance.LoadScene(sceneName);
        }
    }
}
