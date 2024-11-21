using UnityEngine.AI;

namespace State_machine.MovingSM.States {
    public class PatrolState : BaseState {
        protected MovingSM movingSM;
    
        public PatrolState(string name, MovingSM stateMachine): base(name, stateMachine) {
            movingSM = stateMachine;
        }

        public override void Enter() {
            base.Enter();
            NavMeshAgent navMeshAgent = movingSM.GetNavMeshAgent();
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if(movingSM.IsDialogue)
                movingSM.ChangeState(movingSM.dialogueState);
        }

        public override void Exit() {
            base.Exit();
        }
    }
}