using System;
using System.Collections;
using Audio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardBattles.Managers {
    public class EndGameManager : MonoBehaviour {
        [SerializeField] private Text endGameText;
        private void OnEnable() {
            quitBattles.AddListener(SceneSwitcher.Instance.ExitOutOfBattles);
        }
        

        private void OnDisable() {
            quitBattles.RemoveListener(SceneSwitcher.Instance.ExitOutOfBattles);
        }

        [SerializeField, Required] private CanvasGroup rayCastBlocker;
        [SerializeField] public UnityEvent<bool> endGameEventVisual;
        private bool animationEnded = false;

        private bool gameWon = false;
        public void EndGame(bool isPlayersHero) {
            StartCoroutine(EndGameCoroutine(isPlayersHero));
            StartCoroutine(GameSlowDown());
        }

        private IEnumerator EndGameCoroutine(bool isPlayersHero) {
            rayCastBlocker.blocksRaycasts = true;
            endGameEventVisual?.Invoke(!isPlayersHero);
            var clipName = isPlayersHero ? "Lose" : "Win";
            
            var x = AudioCollection.Instance.GetClip(clipName);
            AudioManager.Instance.Play(x);

            StartCoroutine(isPlayersHero ? LoseGame() : WinGame());
            
            yield return new WaitForSecondsRealtime(endGameSlowDownTime);
            
            animationEnded = true;
        }

        private IEnumerator WinGame() {
            endGameText.text = "YOU WIN";
            gameWon = true;
            yield return StartCoroutine(IncreaseTextAlpha());
        }

        private IEnumerator LoseGame() {
            endGameText.text = "YOU LOSE";
            gameWon = false;
            yield return StartCoroutine(IncreaseTextAlpha());
        }

        
        [SerializeField] private float fadeDuration = 3f;
        [SerializeField] private Ease fadeEase = Ease.OutCubic;
        private IEnumerator IncreaseTextAlpha() {
           yield return rayCastBlocker.DOFade(1, fadeDuration)
                .SetEase(fadeEase)
                .WaitForCompletion();
        }
        [SerializeField]
        private UnityEvent<bool> quitBattles;
        public void QuitGame() {
            if(!animationEnded) return;
            Debug.Log("quit battles  invoked");
            quitBattles?.Invoke(gameWon);
        }

        [BoxGroup("SlowDown"), SerializeField] private float endGameSlowDownFinalTimeScaleValue = 0.5f;
        [BoxGroup("SlowDown"), SerializeField] private float endGameSlowDownTime = 2f;
        [BoxGroup("SlowDown"), SerializeField] private Ease endGameSlowDownEase;

        [Button]
        private IEnumerator GameSlowDown() {
            yield return DOTween
                .To(() => Time.timeScale,
                    x => Time.timeScale = x,
                    endGameSlowDownFinalTimeScaleValue,
                    endGameSlowDownTime)
                .SetEase(endGameSlowDownEase);
        }
    }
}