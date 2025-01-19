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

        
        
        [SerializeField]
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
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
        }

        private void Start() {
            var childrenButtons = GetComponentsInChildren<Button>();
            foreach (var button in childrenButtons) {
                if (!buttons.Contains(button))
                    buttons.Add(button);
            }
            ButtonsEnabled(false);

            drawButton.onClick.AddListener(DrawButtonClickHandler);
            endTurnButton.onClick.AddListener(EndTurnButtonClickHandler);
            StartCoroutine(CheckForButtonEnabled());
        }


        private IEnumerator CheckForButtonEnabled() {
            do {
                var isPlayersTurn = TurnManager.Instance.isPlayersTurn;
                if (buttonsEnabled != isPlayersTurn) {
                    if (isPlayersTurn) {
                        yield return new WaitForSeconds(1f);
                    }
                    ButtonsEnabled(isPlayersTurn);

                }

                yield return new WaitForSeconds(0.1f);
            } while (!TurnManager.Instance.gameHasEnded);
        }

        private void ButtonsEnabled(bool value) {
            buttonsEnabled = value;
            foreach (var button in buttons) {
                button.enabled = value;
                button.interactable = value;
            }
        }

        private void DrawButtonClickHandler() {
            if (buttonCooldown) return;

            drawButtonEvent?.Invoke();
            StartCoroutine(ButtonCooldownRoutine());
            StartCoroutine(PressButtonVisualCoroutine(drawButton));
        }
        [SerializeField]
        private float endTurnButtonStartTurnDelay = 0.2f;
        private void EndTurnButtonClickHandler() {
            if (buttonCooldown) return;
            ButtonsEnabled(false);

            StartCoroutine(EndTurnButtonClickCoroutine());
            StartCoroutine(ButtonCooldownRoutine());
            StartCoroutine(PressButtonVisualCoroutine(endTurnButton, true));
        }

        private IEnumerator EndTurnButtonClickCoroutine() {
            yield return new WaitForSeconds(endTurnButtonStartTurnDelay);
            endTurnButtonEvent?.Invoke();
        }

        [SerializeField] private float spriteOffDuration = 0.5f;
        [SerializeField] public float textMoveDownAmount = 3f;

        private IEnumerator PressButtonVisualCoroutine(Button button, bool isEndTurnButton = false) {
            StartCoroutine(ButtonSound());

            ButtonVisual(button, false);


            yield return new WaitForSeconds(spriteOffDuration);
            if (isEndTurnButton) {
                yield return new WaitUntil(() => !TurnManager.Instance.isPlayersTurn);
                yield return new WaitUntil(() => TurnManager.Instance.isPlayersTurn);
            }
            ButtonVisual(button, true);
        }

        
        // ReSharper disable Unity.PerformanceAnalysis
        private void ButtonVisual(Button button, bool buttonUp) {
            
            var x = button.GetComponent<ButtonTextVal>();
            x.OverrideButtonSprite(buttonUp);
            /*button.image.overrideSprite = buttonUp ? null : spriteOff;
            var rectTransform = button.GetComponent<ButtonTextVal>().rectTransformText;
            rectTransform.position +=
                buttonUp ? new Vector3(0, textMoveDownAmount, 0) : new Vector3(0, -textMoveDownAmount, 0);*/
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