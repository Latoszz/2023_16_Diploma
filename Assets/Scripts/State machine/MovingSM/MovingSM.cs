using System.Collections.Generic;
using EnemyScripts;
using State_machine.MovingSM.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace State_machine.MovingSM {
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
        [SerializeField] private List<Transform> waypoints;
        
        [Header("Enemy")]
        [SerializeField] private bool isEnemy;
        public bool IsEnemy => isEnemy;
        [SerializeField] private Transform faceWaypoint;
    
        private NavMeshAgent navMeshAgent;
        private NavMeshAgent playerNavMeshAgent;
        private GameObject player;
        private Enemy enemy;
        private int currentWaypointIndex;
        private bool waiting = false;
        public bool IsDialogue { get; set; } = false;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerNavMeshAgent = player.GetComponent<NavMeshAgent>();

            if (isEnemy) {
                enemy = GetComponent<Enemy>();
            }

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
        
        public NavMeshAgent GetPlayerNavMeshAgent() {
            return playerNavMeshAgent;
        }

        public GameObject GetPlayer() {
            return player;
        }

        public float GetWaitTime() {
            return waitTimeSeconds;
        }

        public List<Transform> GetWaypoints() {
            return waypoints;
        }

        public bool IsWaiting() {
            return waiting;
        }

        public void SetWaiting(bool value) {
            waiting = value;
        }

        public Enemy GetEnemy() {
            return enemy;
        }
        
        public Transform GetFaceWaypoint() {
            return faceWaypoint;
        }
        
    
        public void OnPointerClick(PointerEventData eventData) {
            if (isEnemy && enemy.GetState() == EnemyState.Locked)
                return;

            if (Vector3.Distance(player.transform.position, navMeshAgent.transform.position) < detectionDistance)
                IsDialogue = true;
        }
    }
}
