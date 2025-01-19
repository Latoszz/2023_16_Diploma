using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UI.NPC {
    public class ShowName: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private string messageToShow;
        [SerializeField] private RectTransform nameWindow;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private float detectionRadius = 3f;

        private GameObject player;

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionRadius) {
                ShowMessage();
            }
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            HideMessage();
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