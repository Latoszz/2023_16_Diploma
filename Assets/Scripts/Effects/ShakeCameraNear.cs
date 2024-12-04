using System.Collections;
using CameraScripts;
using InputScripts;
using Tutorial;
using UI.Inventory;
using UnityEngine;

namespace Effects {
    public class ShakeCameraNear: MonoBehaviour {
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private int cameraShakeDuration = 5;
        
        private IEnumerator StartShaking() {
            yield return new WaitUntil(() => !InventoryController.Instance.IsOpen());
            InputManager.Instance.DisableAllInput();
            cameraShake.enabled = true;
            TutorialDialogue.Instance.DisplayNextSentence();
            StartCoroutine(ShakeCameraForSeconds(cameraShakeDuration));
        }

        private IEnumerator ShakeCameraForSeconds(int seconds) {
            yield return new WaitForSeconds(seconds);
            cameraShake.enabled = false;
            InputManager.Instance.EnableAllInput();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(StartShaking());
            }
        }
    }
}