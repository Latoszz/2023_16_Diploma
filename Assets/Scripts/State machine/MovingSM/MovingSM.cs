using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MovingSM : StateMachine, IPointerClickHandler {
    [SerializeField] private float detectionDistance = 8;
    
    [HideInInspector]
    public IdleState idleState;
    [HideInInspector]
    public WalkingState walkingState;
    [HideInInspector]
    public WaitingState waitingState;
    [HideInInspector]
    public DialogueState dialogueState;
    
    [SerializeField] private float waitTimeSeconds;
    [SerializeField] private Transform[] waypoints;
    
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private int currentWaypointIndex;
    private bool waiting = false;
    public bool IsDialogue { get; set; } = false;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        idleState = new IdleState(this);
        walkingState = new WalkingState(this);
        waitingState = new WaitingState(this);
        dialogueState = new DialogueState(this);
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public int GetCurrentWaypointIndex() {
        return currentWaypointIndex;
    }

    public void SetCurrentWaypointIndex(int value) {
        currentWaypointIndex = value;
    }

    public NavMeshAgent GetNavMeshAgent() {
        return navMeshAgent;
    }

    public GameObject GetPlayer() {
        return player;
    }

    public float GetWaitTime() {
        return waitTimeSeconds;
    }

    public Transform[] GetWaypoints() {
        return waypoints;
    }

    public bool IsWaiting() {
        return waiting;
    }

    public void SetWaiting(bool value) {
        waiting = value;
    }

    public void DialogueChangeToFalse() {
        IsDialogue = false;
    }
    public void OnPointerClick(PointerEventData eventData) {
        if(Vector3.Distance(player.transform.position, navMeshAgent.transform.position) < detectionDistance)
            IsDialogue = true;
    }
}
