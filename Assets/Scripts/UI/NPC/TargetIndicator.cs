using UnityEngine;

namespace UI.NPC {
    public class TargetIndicator : MonoBehaviour {
        [SerializeField] private Transform target;
        [SerializeField] private float rotationSpeed;

        private RectTransform pointerRectTransform;
        private Camera mainCamera;

        private void Awake() {
            pointerRectTransform = GetComponent<RectTransform>();
            mainCamera = Camera.main;
        }

        private void Update() {
            Vector3 toPosition = mainCamera.WorldToScreenPoint(target.position);
            Vector3 fromPosition = transform.position;
            fromPosition.z = 0f;
            Vector3 direction = (toPosition - fromPosition).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
            pointerRectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
