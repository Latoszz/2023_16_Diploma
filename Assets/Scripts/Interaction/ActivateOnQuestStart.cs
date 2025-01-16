using System.Collections;
using Esper.ESave;
using Events;
using QuestSystem;
using SaveSystem;
using UI;
using UnityEngine;

namespace Interaction {
    public class ActivateOnQuestStart : MonoBehaviour, ISavable {
        [SerializeField] private QuestInfoSO questInfoSo;
        [SerializeField] private GameObject objectToActivate;
        [SerializeField] private bool waitForFading;
        private bool activated;
        
        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += Activate;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= Activate;
        }

        private void Start() {
            objectToActivate.SetActive(activated);
        }

        private void Activate(string questId) {
            if (questId == questInfoSo.id) {
                if (waitForFading) {
                    StartCoroutine(ActivateCoroutine());
                }
                else {
                    objectToActivate.SetActive(true);
                    activated = true;
                }
            }
        }

        private IEnumerator ActivateCoroutine() {
            yield return new WaitUntil(() => FadeToBlack.Instance.FadingFinished);
            objectToActivate.SetActive(true);
            activated = true;
        }
        
        public void PopulateSaveData(SaveFile saveFile) {
            saveFile.AddOrUpdateData("ActivatedObject_" + objectToActivate.name, activated);
        }

        public void LoadSaveData(SaveFile saveFile) {
            if(saveFile.HasData("ActivatedObject_" + objectToActivate.name))
                activated = saveFile.GetData<bool>("ActivatedObject_" + objectToActivate.name);
        }
    }
}
