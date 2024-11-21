using DG.Tweening;
using UnityEngine;

namespace UI {
    public class ShowIndicator : MonoBehaviour {
        [SerializeField] private GameObject icon;
        [SerializeField] private float floatHeight = 0.5f;
        [SerializeField] private float floatDuration = 1f;

        private Tween floatingTween;
        private void OnEnable() {
            Float();
        }
    
        private void OnDisable() {
            if (floatingTween != null && floatingTween.IsActive()) {
                floatingTween.Kill();
            }
        }

        private void Float() {
            floatingTween = icon.transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void ShowIcon() {
            icon.SetActive(true);
        }
    
        public void HideIcon() {
            icon.SetActive(false);
        }
    }
}
