using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Objects {
    public class ObjectClickHandler : MonoBehaviour {
        public static ObjectClickHandler Instance = null;
        [SerializeField] private InputAction mouseClickAction;
    
        private Camera mainCamera;
        private GameObject clickedObject;

        public bool isActive;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
        }

        private void Start() {
            mainCamera = Camera.main;
        }

        private void OnEnable() {
            mouseClickAction.Enable();
            mouseClickAction.performed += SetObject;
        }

        private void OnDisable() {
            mouseClickAction.performed -= SetObject;
            mouseClickAction.Disable();
        }

        private void SetObject(InputAction.CallbackContext context) {
            if (isActive) {
                Ray ray = mainCamera.ScreenPointToRay( Input.mousePosition );
		
                if(Physics.Raycast( ray, out RaycastHit hit)) {
                    clickedObject = hit.collider.gameObject;
                } 
            }
        }

        public void SetObject(GameObject newObject) {
            clickedObject = newObject;
        }
    
        public GameObject GetObject() {
            return clickedObject;
        }

        public void EnableClickDetection() {
            isActive = true;
        }
    
        public void DisableClickDetection() {
            isActive = false;
        }
    }
}