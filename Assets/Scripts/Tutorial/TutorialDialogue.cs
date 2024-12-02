using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Audio;
using InputScripts;
using TMPro;
using UI.Dialogue;
using UI.HUD;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Tutorial {
    public class TutorialDialogue: MonoBehaviour, IPointerClickHandler {
        [Header("References")] 
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TMP_Text dialogueText;
    
        [SerializeField] private float typingSpeed;

        [Header("Audio")] 
        [SerializeField] private DialogueAudioConfig defaultAudioConfig;
        [SerializeField] private bool makePredictable;
        private AudioSource audioSource;

        [SerializeField] private List<DialogueText> tutorialTexts;
        

        private Queue<string> sentences = new Queue<string>();
        private string sentence;
        private DialogueText dialogue;
    
        private bool wasSkipped;
        private bool isTyping;
        private bool conversationEnded;
        private bool dialogueClosed;
        public bool DialogueClosed => dialogueClosed;

        public static TutorialDialogue Instance;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }

            audioSource = this.gameObject.GetComponent<AudioSource>();
        }

        private void Start() {
            DisplaySentence(tutorialTexts[0]);
        }
    
        public void DisplaySentence(DialogueText dialogue) {
            this.dialogue = dialogue;
            dialogueClosed = false;
        
            if (sentences.Count == 0) {
                if (!conversationEnded) {
                    StartConversation(dialogue);
                }
                else if (conversationEnded && !isTyping) {
                    EndConversation();
                    return;
                }
            }

            if (!isTyping) {
                sentence = sentences.Dequeue();
                dialogueText.text = sentence;
                ShowDialogue();
            }
            else {
                ShowAllText();
            }
        
            if (sentences.Count == 0) {
                conversationEnded = true;
            }
        }

        private void StartConversation(DialogueText dialogue) {
            if (!dialoguePanel.activeSelf) {
                dialoguePanel.SetActive(true);
            }
            SetDialogue(dialogue);
            HUDController.Instance.HideHUD();
        }

        private void EndConversation() {
            conversationEnded = false;
            sentences.Clear();
            HideDialogue();
            HUDController.Instance.ShowHUD();
        }
    
        private void ShowDialogue() {
            isTyping = false;
            InputManager.Instance.DisableInput();
            StopAllCoroutines();
            StartCoroutine(TypeSentence());
        }
    
        public void HideDialogue() {
            InputManager.Instance.EnableInput();
            dialogueClosed = true;
            dialoguePanel.SetActive(false);
        }
    
        private void SetDialogue(DialogueText dialogue) {
            foreach (string sentence in dialogue.Sentences) {
                sentences.Enqueue(sentence);
            }
        }
    
        private IEnumerator TypeSentence() {
            isTyping = true;
            string extractedSentence = ExtractText(sentence);
            
            for (int i = 0; i < sentence.Length; i++) {
                if(i < extractedSentence.Length)
                    PlayDialogueSound(i, sentence[i]);
                dialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(1/typingSpeed);
            }
            audioSource.Stop();
            isTyping = false;
        }

        private string ExtractText(string originalSentence) {
            return Regex.Replace(originalSentence, "<.*?>", String.Empty);
        }

        private void ShowAllText() {
            StopAllCoroutines();
            dialogueText.text = sentence;
            audioSource.Stop();
            isTyping = false;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if(dialoguePanel.activeSelf)
                DisplaySentence(dialogue);
        }

        private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter) {
            AudioClip[] dialogueAudios = defaultAudioConfig.dialogueAudios;
            int frequencyLevel = defaultAudioConfig.frequencyLevel;
            float minPitch = defaultAudioConfig.minPitch;
            float maxPitch = defaultAudioConfig.maxPitch;
            bool stopAudioSource = defaultAudioConfig.stopAudioSource;
        
            if (currentDisplayedCharacterCount % frequencyLevel == 0) {
                if(stopAudioSource)
                    audioSource.Stop();

                AudioClip audioClip = null;
                if (makePredictable) {
                    int hashCode = (int)currentCharacter + 10000;
                    int index = hashCode % dialogueAudios.Length;
                    audioClip = dialogueAudios[index];

                    int minPitchInt = (int)(minPitch * 100);
                    int maxPitchInt = (int)(maxPitch * 100);
                    int pitchRangeInt = maxPitchInt - minPitchInt;
                    if (pitchRangeInt != 0) {
                        int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                        float predictablePitch = predictablePitchInt / 100f;
                        audioSource.pitch = predictablePitch;
                    }
                    else {
                        audioSource.pitch = minPitch;
                    }
                }
                else {
                    int audioIndex = Random.Range(0, dialogueAudios.Length);
                    audioClip = dialogueAudios[audioIndex];
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                }
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}