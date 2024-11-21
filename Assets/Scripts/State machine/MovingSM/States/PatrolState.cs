using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace State_machine.MovingSM.States {
    public class PatrolState : BaseState {
        protected MovingSM movingSM;
        protected NavMeshAgent navMeshAgent;
        protected List<Transform> waypoints;
        protected int currentWaypointIndex;
    
        public PatrolState(string name, MovingSM stateMachine): base(name, stateMachine) {
            movingSM = stateMachine;
        }

        public override void Enter() {
            base.Enter();
            navMeshAgent = movingSM.GetNavMeshAgent();
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            waypoints = movingSM.GetWaypoints();
            currentWaypointIndex = movingSM.GetCurrentWaypointIndex();
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