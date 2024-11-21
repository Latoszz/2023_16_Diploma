using EnemyScripts;
using Esper.ESave;
using UnityEngine;

namespace State_machine.MovingSM.States {
    public class IdleState : PatrolState {
        private Transform faceWaypoint;
        public IdleState(MovingSM stateMachine) : base("Idle", stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            faceWaypoint = movingSM.GetFaceWaypoint();
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if (movingSM.IsEnemy && movingSM.GetEnemy().GetState() == EnemyState.Defeated) {
                return;
            }
            stateMachine.ChangeState(((MovingSM)stateMachine).waitingState);
        }

        public override void UpdatePhysics() {
            base.UpdatePhysics();
            if (movingSM.IsEnemy && movingSM.GetEnemy().GetState() == EnemyState.Defeated) {
                Transform firstWaypoint = waypoints[0];
                navMeshAgent.transform.position = firstWaypoint.position;
                movingSM.dialogueState.RotateTowards(navMeshAgent.transform, faceWaypoint);
                //navMeshAgent.enabled = false;
            }
        }

        public override void Exit() {
            base.Exit();
        }
    }
}
