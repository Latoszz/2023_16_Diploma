using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Infos {
    public class InfoPopup : MonoBehaviour {
        [Header("Time")] 
        [SerializeField] protected int secondsToDisappearInt = 5;
        [SerializeField] protected float fadeTime = 1f;

        [Header("Audio")] 
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
    
        [Header("Setup")]
        [SerializeField] private RectTransform infoPopupPanel;
        [SerializeField] protected TMP_Text infoTitle;
        [SerializeField] protected TMP_Text infoDescription;

        private AudioSource audioSource;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }
    
        protected void PanelFadeIn(float x, float y) {
            infoPopupPanel.DOAnchorPos(new Vector2(x, y), fadeTime, false).SetEase(Ease.InOutQuint);
            audioSource.clip = openSound;
            audioSource.Play();
        }

        private void PanelFadeOut(float x, float y) {
            audioSource.clip = closeSound;
            audioSource.Play();
            infoPopupPanel.DOAnchorPos(new Vector2(x, y), fadeTime, false).SetEase(Ease.InOutQuint);
        }

        protected IEnumerator DisappearAfterSecondsInt(int seconds, float x, float y) {
            yield return new WaitForSeconds(seconds);
            PanelFadeOut(x, y);
        }
    }
}
