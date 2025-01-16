using TMPro;
using UnityEngine;

namespace UI.Infos {
    public class Popup: MonoBehaviour {
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TMP_Text text;

        public void OpenPopup(string textToDisplay) {
            text.text = textToDisplay;
            infoPanel.SetActive(true);
        }
        
        public void ClosePopup() {
            infoPanel.SetActive(false);
        }
    }
}