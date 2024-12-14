using System.Collections;
using InputScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial {
    public class TutorialPlayer : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private Animator animator;
        private static readonly int TutorialStart = Animator.StringToHash("tutorialStart");
        private static readonly int StandUp = Animator.StringToHash("standUp");
        private bool standingUp;
        private bool playerUnlocked;
        public bool PlayerUnlocked => playerUnlocked;

        private void Awake() {
            animator.SetTrigger(TutorialStart);
        }

        private void Start() {
            InputManager.Instance.DisableAllInput();
        }
    
        public void OnPointerClick(PointerEventData eventData) {
            if (standingUp)
                return;
            animator.SetTrigger(StandUp);
            standingUp = true;
            StartCoroutine(WaitForAnimationToFinish());
        }

        private IEnumerator WaitForAnimationToFinish() {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("Stand up") || stateInfo.normalizedTime < 1f) {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
            animator.SetTrigger(StandUp);
            InputManager.Instance.EnableMoveInput();
            playerUnlocked = true;
        }
    }
}
