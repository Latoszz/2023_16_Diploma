using System.Collections;
using DG.Tweening;
using Events;
using QuestSystem;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class FadeToBlack : MonoBehaviour {
        [SerializeField] private QuestInfoSO questInfoSo;
        [SerializeField] private Image fadeImage;
        [Range(0, 5)] [SerializeField] private float duration = 1f;

        public static FadeToBlack Instance;
        public bool FadingFinished;
        
        private void OnEnable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += FadeScreen;
        }

        private void OnDisable() {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= FadeScreen;
        }

        private void Awake() {
            Instance = this;
        }
        
        private void Start() {
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        private void FadeScreen(string questId) {
            if (questId == questInfoSo.id) {
                FadingFinished = false;
                StartCoroutine(FadeCoroutine());
            }
        }

        private IEnumerator FadeCoroutine() {
            yield return new WaitUntil(() => DialogueController.Instance.DialogueClosed);
            Fade();
        }
        
        private void Fade() {
            fadeImage.DOFade(1f, duration).OnComplete(() =>
            {
                fadeImage.DOFade(0f, duration);
                FadingFinished = true;
            });
        }
    }
}
