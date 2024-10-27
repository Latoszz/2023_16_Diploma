using System.Collections;
using System.Collections.Generic;
using Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardBattles.Managers {
    public class ButtonManager : MonoBehaviour {
        public static ButtonManager Instance { get; private set; }
        private bool buttonsEnabled;
        private bool buttonCooldown = false;


        //[SerializeField] private Sprite spriteOn;
        [SerializeField] private Sprite spriteOff;

        private List<Button> buttons = new List<Button>();

        [SerializeField] private float buttonCooldownTime = 0.5f;

        [BoxGroup("End Turn"), SerializeField]
        private Button endTurnButton;

        [BoxGroup("End Turn"), SerializeField]
        private UnityEvent endTurnButtonEvent;

        [BoxGroup("Draw"), SerializeField]
        private Button drawButton;

        [BoxGroup("Draw"), SerializeField]
        private UnityEvent drawButtonEvent;

        private void Awake() {
            if (Instance is null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            var childrenButtons = GetComponentsInChildren<Button>();
            foreach (var button in childrenButtons) {
                if (!buttons.Contains(button))
                    buttons.Add(button);
            }

            drawButton.onClick.AddListener(DrawButtonClickHandler);
            endTurnButton.onClick.AddListener(EndTurnButtonClickHandler);
            StartCoroutine(CheckForButtonEnabled());
        }


        private IEnumerator CheckForButtonEnabled() {
            do {
                if (buttonsEnabled != TurnManager.Instance.isPlayersTurn)
                    ButtonsEnabled(TurnManager.Instance.isPlayersTurn);
                yield return new WaitForSeconds(0.1f);
            } while (!TurnManager.Instance.gameHasEnded);
        }

        private void ButtonsEnabled(bool value) {
            buttonsEnabled = value;
            foreach (var button in buttons) {
                button.interactable = value;
            }
        }

        private void DrawButtonClickHandler() {
            if (buttonCooldown) return;

            drawButtonEvent?.Invoke();
            StartCoroutine(ButtonCooldownRoutine());
            StartCoroutine(PressButtonVisual(drawButton));
        }

        private void EndTurnButtonClickHandler() {
            if (buttonCooldown) return;

            endTurnButtonEvent?.Invoke();
            StartCoroutine(ButtonCooldownRoutine());
            StartCoroutine(PressButtonVisual(endTurnButton));
        }

        [SerializeField] private float spriteOffDuration = 0.5f;
        [SerializeField] private float textMoveDownAmount = 3f;
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator PressButtonVisual(Button button) {
            StartCoroutine(ButtonSound());
            
            button.image.overrideSprite = spriteOff;
            var rectTransform = button.GetComponent<ButtonTextVal>().text.GetComponent<RectTransform>();
            rectTransform.position += new Vector3(0,-textMoveDownAmount,0);
            
            yield return new WaitForSeconds(spriteOffDuration);
            
            rectTransform.position -= new Vector3(0,-textMoveDownAmount,0);
            button.image.overrideSprite = null;
        }

        [SerializeField] private string buttonClickSoundString;
        private IEnumerator ButtonSound() {
            var clip = AudioCollection.Instance.GetClip(buttonClickSoundString);
            AudioManager.Instance.PlayWithVariation(clip);
            yield return null;
        }


        private IEnumerator ButtonCooldownRoutine() {
            buttonCooldown = true;
            yield return new WaitForSeconds(buttonCooldownTime);
            buttonCooldown = false;
        }

        //TODO add some fancy shmansy OnHover, OnHoverExit Actions that allow mana to highlight
    }
}