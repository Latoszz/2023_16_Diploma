using System;
using System.Collections;
using CameraScripts;
using Effects;
using Esper.ESave;
using Events;
using InputScripts;
using Interaction.Scene;
using SaveSystem;
using Tutorial;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Statue : MonoBehaviour, IPointerClickHandler, ISavable {
        [SerializeField] private ShowOutline showOutlineScript;
        [SerializeField] private GameObject shakeCamera;
        [SerializeField] private string sceneName;

        private bool isActive;

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
            shakeCamera.SetActive(true);
            isActive = true;
        }

        public void OnPointerClick(PointerEventData eventData) {
            //if (entered) return;
            //entered = true;
            //SaveManager.Instance.SaveGame();
            if (!isActive || shakeCamera.activeSelf)
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
