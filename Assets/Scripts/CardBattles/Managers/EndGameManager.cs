using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CardBattles.Managers {
    public class EndGameManager : MonoBehaviour {
        private void OnEnable() {
            quitBattles.AddListener(SceneSwitcher.Instance.ExitOutOfBattles);
        }

        private void OnDisable() {
            quitBattles.RemoveListener(SceneSwitcher.Instance.ExitOutOfBattles);
        }

        [SerializeField, Required] private CanvasGroup rayCastBlocker;
        [SerializeField] public UnityEvent<bool> endGameEventVisual;
        private bool animationEnded = false;
        public void EndGame(bool isPlayersHero) {
            StartCoroutine(EndGameCoroutine(isPlayersHero));
            StartCoroutine(GameSlowDown());
        }

        private IEnumerator EndGameCoroutine(bool isPlayersHero) {
            rayCastBlocker.blocksRaycasts = true;
            endGameEventVisual?.Invoke(!isPlayersHero);
            yield return new WaitForSecondsRealtime(endGameSlowDownTime);
            if (isPlayersHero) {
                yield return LoseGame();
            }
            else {
                yield return WinGame();
            }

            animationEnded = true;
        }

        private IEnumerator WinGame() {
            Debug.Log("Congrats you won");
            
            yield return null;
        }

        private IEnumerator LoseGame() {
            Debug.Log("Boohoo :(  LOSER");
            yield return null;
        }

        private UnityEvent quitBattles;
        public void QuitGame() {
            if(!animationEnded) return;
            Debug.Log("quit battles invoked");
            quitBattles?.Invoke();
        }
        
        
        


        [BoxGroup("SlowDown"), SerializeField] private float endGameSlowDownFinalTimeScaleValue = 0.1f;
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