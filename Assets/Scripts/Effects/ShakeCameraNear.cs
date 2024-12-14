using System.Collections;
using CameraScripts;
using InputScripts;
using Tutorial;
using UI.Inventory;
using UnityEngine;

namespace Effects {
    public class ShakeCameraNear: MonoBehaviour {
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private float cameraShakeDuration = 5;

        private void Start() {
            cameraShake.shakeDuration = cameraShakeDuration;
        }
        
        private IEnumerator StartShaking() {
            yield return new WaitUntil(() => !InventoryController.Instance.IsOpen());
            InputManager.Instance.DisableMoveInput();
            InputManager.Instance.DisableInventory();
            cameraShake.enabled = true;
            StartCoroutine(ShakeCameraForSeconds(cameraShakeDuration));
        }

        private IEnumerator ShakeCameraForSeconds(float seconds) {
            yield return new WaitForSeconds(seconds);
            cameraShake.enabled = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(StartShaking());
            }
        }
    }
}