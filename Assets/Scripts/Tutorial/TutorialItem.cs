using System;
using Events;
using InputScripts;
using UI.Dialogue;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialItem : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private DialogueText dialogueText;
        [SerializeField] private Item item;
        [SerializeField] private TutorialScene tutorialScene;

        private void Update() {
            if(tutorialScene == TutorialScene.Overworld)
                item.enabled = TutorialDialogue.Instance.CurrentDialogue == dialogueText;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (TutorialDialogue.Instance.CurrentDialogue != dialogueText) {
                return;
            }

            switch (tutorialScene) {
                case TutorialScene.Overworld:
                    TutorialDialogue.Instance.DisplayNextSentence();
                    InputManager.Instance.EnableInventory();
                    GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
                    GameEventsManager.Instance.TutorialEvents.UnlockStatue();
                    GameEventsManager.Instance.TutorialEvents.UnlockInventory();
                    break;
                case TutorialScene.RoomUnderStatue:
                    TutorialDialogue.Instance.DisplayNextSentence();
                    InputManager.Instance.EnableQuestPanel();
                    GameEventsManager.Instance.ItemEvents.ItemReward(item.GetName());
                    GameEventsManager.Instance.TutorialEvents.UnlockQuests();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
