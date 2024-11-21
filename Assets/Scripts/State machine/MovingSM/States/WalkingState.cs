using UnityEngine;
using UnityEngine.AI;

namespace State_machine.MovingSM.States {
    public class WalkingState : PatrolState {
        private NavMeshAgent navMeshAgent;
        private Transform[] waypoints;
        private int currentWaypointIndex;
        private Vector3 currentWaypoint;
        public WalkingState(MovingSM stateMachine) : base("Walking", stateMachine) {
        
        }

        public override void Enter() {
            base.Enter();
            navMeshAgent = movingSM.GetNavMeshAgent();
            waypoints = movingSM.GetWaypoints();
            currentWaypointIndex = movingSM.GetCurrentWaypointIndex();
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            UpdateWaypoint();
            if (Vector3.Distance(navMeshAgent.transform.position, currentWaypoint) < 0.6) {
                SetWaypoint();
                movingSM.SetWaiting(true);
                movingSM.ChangeState(movingSM.waitingState);
            }
            else {
                Move();
            }
        }
    
        private void Move() {
            navMeshAgent.SetDestination(currentWaypoint);
        }
    
        private void SetWaypoint() {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            movingSM.SetCurrentWaypointIndex(currentWaypointIndex);
        }

        private void UpdateWaypoint() {
            currentWaypoint = waypoints[currentWaypointIndex].position;
            currentWaypoint.y = navMeshAgent.transform.position.y;
        }
    }
}
