using System.Collections;
using InputScripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPlayer : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Animator animator;
    private static readonly int TutorialStart = Animator.StringToHash("tutorialStart");
    private static readonly int StandUp = Animator.StringToHash("standUp");

    private void Awake() {
        animator.SetTrigger(TutorialStart);
    }

    private void Start() {
        InputManager.Instance.DisableInput();
    }


    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log($"Click");
        animator.SetTrigger(StandUp);
        StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish() {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0));
        animator.SetTrigger(StandUp);
        InputManager.Instance.EnableInput();
    }
}
