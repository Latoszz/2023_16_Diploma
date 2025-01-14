using UnityEngine;
using UnityEngine.AI;

namespace State_machine.MovingSM.States {
    public class FollowPlayerState: BaseState {
        private MovingSM movingSM;
        private GameObject player;
        private NavMeshAgent agent;
        static readonly int IsMoving = Animator.StringToHash("isMoving");
        
        public FollowPlayerState(MovingSM stateMachine) : base("Follow", stateMachine) {
            movingSM = stateMachine;
            agent = movingSM.GetNavMeshAgent();
            player = movingSM.GetPlayer();
        }

        public override void Enter() {
            base.Enter();
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if (!movingSM.FollowsPlayer) {
                movingSM.ChangeState(movingSM.walkingState);
            }
        }

        public override void UpdatePhysics() {
            base.UpdatePhysics();
            if (Vector3.Distance(agent.transform.position, player.transform.position) > movingSM.FollowDistance) {
                agent.SetDestination(player.transform.position);
                if(movingSM.GetAnimator() is not null) 
                    movingSM.GetAnimator().SetBool(IsMoving, true);
            }
            else {
                agent.ResetPath();
                if(movingSM.GetAnimator() is not null) 
                    movingSM.GetAnimator().SetBool(IsMoving, false);
            }
        }

        public override void Exit() {
            base.Exit();
        }
    }
}