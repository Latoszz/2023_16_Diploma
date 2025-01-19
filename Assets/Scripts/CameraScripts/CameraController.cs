using System.Collections;
using UnityEngine;

namespace CameraScripts {
    public class CameraController : MonoBehaviour {
        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 0.1f;
    
        [Header("Rotate")]
        [SerializeField] private float rotationSpeed = 5f;

        private UnityEngine.Camera mainCamera;
        private Quaternion initialRotation;
        private float initialZoom;
        private float rightRotation;
        private float leftRotation;

        public static CameraController Instance = null;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }         
            else if (Instance != this) {
                Destroy(gameObject);
            }          
            //DontDestroyOnLoad(gameObject); 
            mainCamera = UnityEngine.Camera.main;
            initialRotation = transform.rotation;
            initialZoom = mainCamera.orthographicSize;
        }

        private bool ZoomIn(float zoomRange) {
            if (mainCamera.orthographicSize > zoomRange) {
                mainCamera.orthographicSize -= zoomSpeed;
                return true;
            }
            return false;
        }
    
        private bool ZoomOut(float zoomRange) {
            if (mainCamera.orthographicSize < zoomRange) {
                mainCamera.orthographicSize += zoomSpeed;
                return true;
            }
            return false;
        }

        private bool RotateRight(float units) {
            if (rightRotation < units) {
                transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime), Space.World);
                rightRotation = initialRotation.y - transform.rotation.y;
                return true;
            }
            return false;
        }
    
        private bool RotateLeft(float units) {
            if (leftRotation < units) {
                transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime), Space.World);
                leftRotation = transform.rotation.y - initialRotation.y;
                return true;
            }
            return false;
        }

        public void SootheIn(float zoom, float rotationUnits) {
            StartCoroutine(In(zoom, rotationUnits));
        }
    
        public void SootheOut(float zoom, float rotationUnits) {
            StartCoroutine(Out(zoom, rotationUnits));
        }

        private IEnumerator In(float zoom, float rotationUnits) {
            while (ZoomIn(zoom) && RotateLeft(rotationUnits)) {
                yield return null;
            }
        }

        private IEnumerator Out(float zoom, float rotationUnits) {
            while (ZoomOut(initialZoom) && RotateRight(rotationUnits)) {
                yield return null;
            }
        }

        public void ResetPosition() {
            transform.rotation = initialRotation;
            mainCamera.orthographicSize = initialZoom;
        }
    }
}
