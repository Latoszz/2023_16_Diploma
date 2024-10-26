using System.Collections;
using CardBattles.Enums;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace CardBattles.Managers {
    public class EndGameManager : MonoBehaviour {
        [SerializeField, Required] private CanvasGroup rayCastBlocker;

        public void EndGame(bool isPlayersHero) {
            StartCoroutine(EndGameCoroutine(isPlayersHero));
            StartCoroutine(GameSlowDown());
        }

        private IEnumerator EndGameCoroutine(bool isPlayersHero) {
            rayCastBlocker.blocksRaycasts = true;
            if (isPlayersHero) {
                yield return LoseGame();
            }
            else {
                yield return WinGame();
            }
        }

        private IEnumerator WinGame() {
            Debug.Log("Congrats you won");
            yield return new WaitForSecondsRealtime(2f);
            if (Application.isEditor)
                EditorApplication.isPlaying = false;
        }

        private IEnumerator LoseGame() {
            EffectVisualsManager.Instance.visual[EffectName.EndOfGame](new Component());
            Debug.Log("Boohoo :(  LOSER");
            yield return new WaitForSecondsRealtime(2f);
            if (Application.isEditor)
                EditorApplication.isPlaying = false;
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