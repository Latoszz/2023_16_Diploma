using System.Collections;
using System.Collections.Generic;
using Audio;
using ScriptableObjects.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour, IPointerClickHandler {
    [Header("Dialogue camera settings")] 
    [SerializeField] private float zoomIntensity = 5f;
    [SerializeField] private float rotationUnits = 0.05f;

    [Header("References")] 
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject nextIcon;
    
    [SerializeField] private float typingSpeed;

    [Header("Audio")] 
    [SerializeField] private DialogueAudioConfig defaultAudioConfig;
    [SerializeField] private bool makePredictable;
    private DialogueAudioConfig currentAudioConfig;
    private AudioSource audioSource;

    private const string HTML_ALPHA = "<color=#00000000>";

    private Queue<string> sentences = new Queue<string>();
    private string sentence;
    private DialogueText dialogue;
    
    private bool wasSkipped;
    private bool isTyping;
    private bool conversationEnded;
    private bool dialogueClosed;
    public bool DialogueClosed => dialogueClosed;

    public static DialogueController Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        audioSource = this.gameObject.AddComponent<AudioSource>();
        currentAudioConfig = defaultAudioConfig;
    }

    public void SetCurrentAudioConfig(DialogueAudioConfig audioConfig) {
        if (audioConfig == null)
            currentAudioConfig = defaultAudioConfig;
        currentAudioConfig = audioConfig;
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
        CameraController.Instance.SootheIn(zoomIntensity, rotationUnits);
    }

    private void EndConversation() {
        conversationEnded = false;
        sentences.Clear();
        HideDialogue();
        HUDController.Instance.ShowHUD();
        CameraController.Instance.SootheOut(7f, rotationUnits);
    }
    
    private void ShowDialogue() {
        isTyping = false;
        nextIcon.SetActive(false);
        InputManager.Instance.DisableInput();
        StopAllCoroutines();
        StartCoroutine(TypeSentence());
    }
    
    private void HideDialogue() {
        InputManager.Instance.EnableInput();
        dialogueClosed = true;
        dialoguePanel.SetActive(false);
    }
    
    private void SetDialogue(DialogueText dialogue) {
        icon.sprite = dialogue.Icon;
        nameText.text = dialogue.NameText;
        foreach (string sentence in dialogue.Sentences) {
            sentences.Enqueue(sentence);
        }
    }
    
    private IEnumerator TypeSentence() {
        isTyping = true;
        dialogueText.text = "";
        string originalText = sentence;
        string displayedText = "";
        int alphaIndex = 0;
        
        foreach (char letter in sentence) {
            if (dialogueText.text != "") {
                PlayDialogueSound(alphaIndex, originalText[alphaIndex]);
            }

            alphaIndex++;
            dialogueText.text = originalText;
            displayedText = dialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            dialogueText.text = displayedText;
            yield return new WaitForSeconds(1/typingSpeed);
        }
        audioSource.Stop();
        nextIcon.SetActive(true);
        isTyping = false;
    }

    private void ShowAllText() {
        StopAllCoroutines();
        dialogueText.text = sentence;
        audioSource.Stop();
        nextIcon.SetActive(true);
        isTyping = false;
    }
    public void OnPointerClick(PointerEventData eventData) {
        if(dialoguePanel.activeSelf)
            DisplaySentence(dialogue);
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter) {
        AudioClip[] dialogueAudios = currentAudioConfig.dialogueAudios;
        int frequencyLevel = currentAudioConfig.frequencyLevel;
        float minPitch = currentAudioConfig.minPitch;
        float maxPitch = currentAudioConfig.maxPitch;
        bool stopAudioSource = currentAudioConfig.stopAudioSource;
        
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
