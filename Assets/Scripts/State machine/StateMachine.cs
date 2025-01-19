using UnityEngine;

namespace State_machine {
    public class StateMachine : MonoBehaviour {
        protected BaseState currentState;

        private void Start() {
            currentState = GetInitialState();
            currentState.Enter();
        }

        private void Update() {
            if(currentState != null)
                currentState.UpdateLogic();
        }

        private void LateUpdate() {
            if(currentState != null)
                currentState.UpdatePhysics();
        }

        public void ChangeState(BaseState newState) {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        protected virtual BaseState GetInitialState() {
            return null;
        }

        public BaseState GetCurrentState() {
            return currentState;
        }
    }
}
