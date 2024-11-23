using UI.Dialogue;
using UnityEngine;
using UnityEngine.AI;

namespace State_machine.MovingSM.States {
    public class DialogueState : BaseState {
        private MovingSM movingSM;
        private NavMeshAgent navMeshAgent;
        private NavMeshAgent playerNavMeshAgent;
    
        public DialogueState(MovingSM stateMachine) : base("Dialogue", stateMachine) {
            movingSM = stateMachine;
        }

        public override void Enter() {
            base.Enter();
            navMeshAgent = movingSM.GetNavMeshAgent();
            navMeshAgent.isStopped = true;
            navMeshAgent.updateRotation = false;

            playerNavMeshAgent = movingSM.GetPlayerNavMeshAgent();
            playerNavMeshAgent.ResetPath();  
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
        
            if (DialogueController.Instance.DialogueClosed) {
                movingSM.IsDialogue = false;
                navMeshAgent.isStopped = false;
                navMeshAgent.updateRotation = true;
                movingSM.ChangeState(movingSM.idleState);
            }
        }

        public override void UpdatePhysics() {
            base.UpdatePhysics();
            FacePlayer();
        }

        public override void Exit() {
            base.Exit();
        }

        private void FacePlayer() {
            Transform player = movingSM.GetPlayer().transform;
            Transform npc = navMeshAgent.transform;
        
            RotateTowards(npc, player);
            RotateTowards(player, npc);
        }

        public void RotateTowards(Transform source, Transform target) {
            navMeshAgent ??= movingSM.GetNavMeshAgent();
            
            Vector3 lookPos = (target.position - source.position).normalized;
            lookPos.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookPos);
            source.rotation = Quaternion.Slerp(source.rotation, lookRotation, Time.deltaTime * navMeshAgent.speed);
        }
    }
}