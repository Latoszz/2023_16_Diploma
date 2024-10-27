using UnityEngine;
using UnityEngine.AI;

public class DialogueState : BaseState {
    private MovingSM movingSM;
    private NavMeshAgent navMeshAgent;
    
    public DialogueState(MovingSM stateMachine) : base("Dialogue", stateMachine) {
        movingSM = stateMachine;
    }

    public override void Enter() {
        base.Enter();
        navMeshAgent = movingSM.GetNavMeshAgent();
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();
        
        if (DialogueController.Instance.DialogueClosed) {
            movingSM.IsDialogue = false;
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

    private void RotateTowards(Transform source, Transform target) {
        Vector3 lookPos = (target.position - source.position).normalized;
        lookPos.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        source.rotation = Quaternion.Slerp(source.rotation, lookRotation, Time.deltaTime * navMeshAgent.speed);
    }
}