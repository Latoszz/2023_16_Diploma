using UnityEngine;

namespace UI {
    public class UILookAt : MonoBehaviour {
        private Camera mainCamera;

        private void Awake() {
            mainCamera = Camera.main;
        }

        private void LateUpdate() {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
