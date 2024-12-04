using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Flicker : MonoBehaviour {
        [SerializeField] private float interval = 1f;
        [SerializeField] private float startDelay = 0.5f;
        [SerializeField] private bool defaultState = true;
        private bool isBlinking = false;
        private Image image;

        private void Awake() {
            image = GetComponent<Image>();
        }

        private void Start() {
            image.enabled = defaultState;
            StartBlink();
        }
        
        private void StartBlink() {
            if (isBlinking)
                return;

            if (image !=null) {
                isBlinking = true;
                InvokeRepeating("ToggleState", startDelay, interval);
            }
        }

        private void ToggleState() {
            image.enabled = !image.enabled;
        }
    }
}
