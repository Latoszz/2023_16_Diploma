using DG.Tweening;
using UnityEngine;

public class ShowQuestIndicator : MonoBehaviour {
    [SerializeField] private GameObject questImage;
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
        floatingTween = questImage.transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowQuestIcon() {
        questImage.SetActive(true);
    }
    
    public void HideQuestIcon() {
        questImage.SetActive(false);
    }
}
