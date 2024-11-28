using Esper.ESave;
using Interaction.Scene;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Statue : MonoBehaviour, IPointerClickHandler, ISavable {
        [SerializeField] private string sceneName;

        private const string StatueSaveID = "StatueSave";
        private bool entered = false;

        private void Start() {
            if (entered) {
                GetComponent<ShowOutline>().enabled = false;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (entered) return;
            entered = true;
            SaveManager.Instance.SaveGame();
            SceneSwitcher.Instance.LoadScene(sceneName);
        }

        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData(StatueSaveID, entered);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if (saveFile.HasData(StatueSaveID)) {
                entered = saveFile.GetData<bool>(StatueSaveID);
            }
        }
    }
}
