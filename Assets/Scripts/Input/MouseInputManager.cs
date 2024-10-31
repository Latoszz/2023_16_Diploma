using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseInputManager : MonoBehaviour, IPointerUpHandler {
    [SerializeField] private InputAction mouseClickAction;
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private GameObject player;
    private Camera mainCamera;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float normalSpeed = 7f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float slowDownRadius = 3f;
    private bool pointerOverUI;
    
    private Vector3 targetPosition;
    private int groundLayer;
    private bool mouseClickEnabled = true;
    [SerializeField] private Animator animator;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Awake() {
        mainCamera = Camera.main;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = player.GetComponent<NavMeshAgent>();
        groundLayer = LayerMask.NameToLayer("Ground");
        targetPosition = transform.position;
    }
    
    void Update() {
        if (pointerOverUI || InventoryController.Instance.IsOpen()) {
            return;
        }
        
        if (Input.GetMouseButton(0)) {
            MoveTowardsCursor();
        }

        // Speed control
        if (navMeshAgent.hasPath) {
            float distance = Vector3.Distance(transform.position, navMeshAgent.destination);

            if (distance > slowDownRadius) {
                navMeshAgent.speed = maxSpeed;
            }
            else {
                navMeshAgent.speed = Mathf.Lerp(normalSpeed, maxSpeed, distance / slowDownRadius);
            }
        }
    }

    void MoveTowardsCursor() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0) {
            var hitPoint = hit.point;
            if (Vector3.Distance(navMeshAgent.transform.position, hitPoint) > stopDistance) {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(hitPoint);
            }
        }
    }

    private void OnEnable() {
        mouseClickAction.Enable();
        mouseClickAction.performed += MovePlayer;
    }

    private void OnDisable() {
        mouseClickAction.performed -= MovePlayer;
        mouseClickAction.Disable();
    }
    
    private void MovePlayer(InputAction.CallbackContext context) {
        if (!mouseClickEnabled)
            return;
        if (pointerOverUI) {
            return;
        }
        SetTargetPoint();
    }
    
    // Call this when the pointer enters a UI element
    public void OnPointerEnter() {
        pointerOverUI = true;
    }

    // Call this when the pointer exits a UI element
    public void OnPointerExit() {
        pointerOverUI = false;
    }
    
    private void SetTargetPoint() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0) {
            targetPosition = hit.point;
            Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            Walk(targetPosition);
            StopAllCoroutines();
            StartCoroutine(Wait());
        }
    }

    private void Walk(Vector3 target) {
        animator.SetBool(IsMoving, true);
        navMeshAgent.SetDestination(target);
    }

    private IEnumerator Wait() {
        bool isDone = false;
        
        while (!isDone) {
            if (!navMeshAgent.pathPending) {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                        isDone = true;
                    }
                }
            }
            yield return null;
        } 
        animator.SetBool(IsMoving, false);
    }

    public void EnableMouseControls() {
        mouseClickEnabled = true;
    }
    
    public void DisableMouseControls() {
        mouseClickEnabled = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log(eventData.position);
        Instantiate(clickEffect, (Vector3)eventData.position + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
    }
}
