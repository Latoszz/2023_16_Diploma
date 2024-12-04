using InputScripts;
using UnityEngine;

namespace CameraScripts {
    public class CameraShake : MonoBehaviour {
        [SerializeField] private Transform camTransform;
        [SerializeField] private float shakeDuration = 0f;
        [SerializeField] private float shakeAmount = 0.7f;
        [SerializeField] private float decreaseFactor = 1.0f;
	
        private Vector3 originalPos;
	
        private void Awake() {
            if (camTransform == null) {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }
	
        private void OnEnable() {
            originalPos = camTransform.localPosition;
            InputManager.Instance.DisableAllInput();
        }

        private void OnDisable() {
            InputManager.Instance.EnableAllInput();
        }

        private void Update() {
            if (shakeDuration > 0) {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
            }
        }
    }
}

