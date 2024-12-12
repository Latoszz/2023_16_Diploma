using System.Collections.Generic;
using DG.Tweening;
using Events;
using InputScripts;
using QuestSystem;
using TMPro;
using UnityEngine;

namespace UI.Infos {
    public class QuestListPanel : MonoBehaviour {
        [Header("Time")] 
        [SerializeField] private float fadeTime = 1f;
        
        [Header("Audio")] 
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
        private AudioSource audioSource;

        [Header("Setup")] 
        [SerializeField] private RectTransform questPanel;
        [SerializeField] private GameObject questList;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private GameObject questInfoPrefab;

        private Dictionary<string, GameObject> listOfQuests;
        public bool IsOpen { get; private set; }

        public static QuestListPanel Instance;
        
        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            audioSource = GetComponent<AudioSource>();
        }

        private void Start() {
            var loadedQuests = QuestManager.Instance.GetQuestList();   
            listOfQuests = new Dictionary<string, GameObject>();
            if (loadedQuests != null) {
                foreach (Quest quest in loadedQuests.Values) {
                    if(quest.state is QuestState.IN_PROGRESS)
                        AddQuestToList(quest.info.id);
                }
            }
        }
    
        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += AddQuestToList;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest += RemoveQuest;
        }
    
        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= AddQuestToList;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest -= RemoveQuest;
        }

        private void AddQuestToList(string questId) {
            QuestInfoSO questInfo = questManager.GetQuestById(questId).info;
            GameObject displayObject = Instantiate(questInfoPrefab, questList.transform, false);
            displayObject.transform.GetChild(0).GetComponent<TMP_Text>().text = questInfo.displayName;
            displayObject.transform.GetChild(1).GetComponent<TMP_Text>().text = questInfo.questDescription;
            listOfQuests.TryAdd(questId, displayObject);
        }

        private void RemoveQuest(string questId) {
            Destroy(listOfQuests[questId]);
            listOfQuests.Remove(questId);
        }
    
        private void PanelFadeIn() {
            IsOpen = true;
            questPanel.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.InOutQuint);
            audioSource.clip = openSound;
            audioSource.Play();
        }

        private void PanelFadeOut() {
            questPanel.DOAnchorPos(new Vector2(500f, 0f), fadeTime, false).SetEase(Ease.InOutQuint);
            IsOpen = false;
            audioSource.clip = closeSound;
            audioSource.Play();
        }

        public void OpenClosePanel() {
            if (IsOpen) {
                Close();
            }
            else {
                InputManager.Instance.DisableInventory();
                InputManager.Instance.DisablePause();
                PanelFadeIn();
            }
        }

        public void Close() {
            InputManager.Instance.EnableInventory();
            InputManager.Instance.EnablePause();
            PanelFadeOut();
        }
    }
}
