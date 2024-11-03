using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-90)]
public class KeyboardInputManager : MonoBehaviour, PlayerControls.IPlayerActionMapActions {

    private PlayerControls playerControls;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float speed;
    
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    private Vector3 movementVector;
    private Vector3 targetDirection;
    private float lerpTime;
    private bool isWalking;

    private Matrix4x4 rotationMatrix;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Awake() {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = player.GetComponent<NavMeshAgent>();
        rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -cameraPivot.rotation.eulerAngles.y));
    }
    
    private void Start() {
        playerControls = InputManager.Instance.playerControls;
        playerControls.PlayerActionMap.SetCallbacks(this);
        playerControls.PlayerActionMap.Enable();
    }
    
    private void Update() {
        movementVector.Normalize();
        targetDirection = Vector3.Lerp(targetDirection, movementVector, speed * Time.deltaTime);
        navMeshAgent.Move(targetDirection * (navMeshAgent.speed * Time.deltaTime));
        Vector3 lookDirection = movementVector;
        
        if (lookDirection != Vector3.zero) {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(lookDirection),
                speed * Time.deltaTime);
        }
    }
    
    public void OnMove(InputAction.CallbackContext context) {
        navMeshAgent.ResetPath();
        if (context.performed)
            isWalking = true;
        if (context.canceled)
            isWalking = false;
        animator.SetBool(IsMoving, isWalking);
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 skewedInput = rotationMatrix.MultiplyPoint3x4(input);
        movementVector = new Vector3(skewedInput.x, 0, skewedInput.y);
    }
    
    public void OnOpenInventory(InputAction.CallbackContext context) {
        if (context.performed) {
            InventoryController inventoryController = InventoryController.Instance;
            if (inventoryController.IsOpen())
                inventoryController.HideInventory();
            else {
                inventoryController.ShowInventory(context);
            }
        }
    }
    
    public void OnPause(InputAction.CallbackContext context) {
        if (context.performed) {
            if (PauseManager.Instance.IsOpen)
                PauseManager.Instance.Close();
            else {
                PauseManager.Instance.Open();
            }
        }
    }

    public void EnableKeyboardControls() {
        playerControls.PlayerActionMap.Enable();
    }
    
    public void DisableKeyboardControls() {
        playerControls.PlayerActionMap.Disable();
    }
    
    public void EnableMovement() {
        playerControls.PlayerActionMap.Move.Enable();
    }
    
    public void DisableMovement() {
        playerControls.PlayerActionMap.Move.Disable();
    }

    public void DisablePause() {
        playerControls.PlayerActionMap.Pause.Disable();
    }
    
    public void EnablePause() {
        playerControls.PlayerActionMap.Pause.Enable();
    }
}