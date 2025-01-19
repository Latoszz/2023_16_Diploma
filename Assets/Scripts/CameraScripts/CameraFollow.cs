using UnityEngine;

namespace CameraScripts {
    public class CameraFollow : MonoBehaviour {
        [SerializeField] private float smoothing = 5f;
        [SerializeField] private Transform target;

        private void Update() {
            Vector3 newPosition = target.position;
            transform.position = Vector3.Lerp (transform.position, newPosition, smoothing * Time.deltaTime);
        }
    }
}
