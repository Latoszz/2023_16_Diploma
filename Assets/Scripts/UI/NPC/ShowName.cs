using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace UI.NPC {
    public class ShowName: MonoBehaviour {
        [SerializeField] private float timeToWait = 0.5f;
        [SerializeField] private string messageToShow;
        [SerializeField] private RectTransform nameWindow;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private GameObject player;
    
        private Camera mainCamera;
        private bool playerNear = false;
        private float hoverTimer = 0f;

        private void Awake() {
            mainCamera = Camera.main;
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            //HideMessage();
        }

        private void Update() {
            /*
        CheckPlayerNear();
        if (playerNear && IsMouseOver()) {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= timeToWait) {
                MoveTextNearCursor();
                ShowMessage();
            }
        }
        else {
            hoverTimer = 0f;
            HideMessage();
        }
        */
        }

        private void CheckPlayerNear() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
            if (distanceToPlayer <= detectionRadius) {
                playerNear = true;
            }
            else {
                playerNear = false;
            }
        }

        private bool IsMouseOver() {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject) {
                return true;
            }
            return false;
        }
    
        private void MoveTextNearCursor() {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            nameWindow.sizeDelta = new Vector2(nameText.preferredWidth > 100 ? 100 : nameText.preferredWidth, nameText.preferredHeight);
            nameWindow.transform.position = new Vector2(mousePosition.x + nameWindow.sizeDelta.x/2, mousePosition.y);
        }
    
        private void ShowMessage() {
            nameText.text = messageToShow;
            nameWindow.gameObject.SetActive(true);
        }

        private void HideMessage() {
            nameWindow.gameObject.SetActive(false);
        }
    }
}