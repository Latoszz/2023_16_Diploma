using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Update() {
        Vector3 newPosition = target.position + offset;
        transform.position = Vector3.Lerp (transform.position, newPosition, smoothing * Time.deltaTime);
    }
}
