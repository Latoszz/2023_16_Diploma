using UnityEngine;

namespace State_machine.MovingSM.States {
    public class WaitingState : PatrolState {
        private float waitCounter;
        private float waitTime;

        public WaitingState(MovingSM stateMachine) : base("Waiting", stateMachine) {
        
        }

        public override void Enter() {
            waitTime = movingSM.GetWaitTime();
            if(movingSM.GetAnimator() is not null)
                movingSM.GetAnimator().SetBool(IsMoving, false);
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if (movingSM.IsWaiting() || movingSM.ReachedDestination){
                Wait();
                movingSM.ReachedDestination = movingSM.GetWaypoints().Count == 1;
            }
            else {
                movingSM.ChangeState(movingSM.walkingState);
            }
        }

        public override void UpdatePhysics() {
            base.UpdatePhysics();
        }

        public override void Exit() {
            base.Exit();
            waitCounter = 0f;
        }

        private void Wait() {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
                movingSM.SetWaiting(false);
        }
    }
}    