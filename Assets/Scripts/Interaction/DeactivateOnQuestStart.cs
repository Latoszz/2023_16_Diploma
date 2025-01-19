using Esper.ESave;
using Events;
using QuestSystem;
using SaveSystem;
using UnityEngine;

namespace Interaction {
    public class DeactivateOnQuestStart : MonoBehaviour, ISavable {
        [SerializeField] private QuestInfoSO questInfoSo;
        [SerializeField] private GameObject objectToDeactivate;
        private bool deactivated;
        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= Activate;
        }

        private void Start() {
            objectToDeactivate.SetActive(!deactivated);
        }

        private void Activate(string questId) {
            if (questId == questInfoSo.id) {
                objectToDeactivate.SetActive(false);
                deactivated = true;
            }
        }

        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData("DeactivatedObject_" + objectToDeactivate.name, deactivated);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if(saveFile.HasData("DeactivatedObject_" + objectToDeactivate.name))
                deactivated = saveFile.GetData<bool>("DeactivatedObject_" + objectToDeactivate.name);
        }
    }
}
