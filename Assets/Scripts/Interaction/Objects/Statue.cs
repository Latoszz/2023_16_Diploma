using Esper.ESave;
using Events;
using Interaction.Scene;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Statue : MonoBehaviour, IPointerClickHandler, ISavable {
        [SerializeField] private ShowOutline showOutlineScript;
        [SerializeField] private string sceneName;

        private const string StatueSaveID = "StatueSave";
        //private bool entered = false;
        private void OnEnable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.TutorialEvents.OnUnlockStatue -= Activate;
        }

        private void Activate() {
            showOutlineScript.enabled = true;
        }

        private void Start() {
            /*
            if (entered) {
                GetComponent<ShowOutline>().enabled = false;
            }
            */
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            //if (entered) return;
            //entered = true;
            //SaveManager.Instance.SaveGame();
            if (showOutlineScript.enabled == false)
                return;
            SceneSwitcher.Instance.LoadScene(sceneName);
        }

        public void PopulateSaveData(SaveFile saveFile) {
            //saveFile.AddOrUpdateData(StatueSaveID, entered);
        }

        public void LoadSaveData(SaveFile saveFile) {
            /*
            if (saveFile.HasData(StatueSaveID)) {
                entered = saveFile.GetData<bool>(StatueSaveID);
            }
            */
        }
    }
}
