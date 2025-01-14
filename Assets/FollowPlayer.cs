using State_machine.MovingSM;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField] private MovingSM movingSM;

    private void Start() {
        
    }

    private void OnEnable() {
        movingSM.FollowPlayer();
    }

    private void OnDisable() {
        movingSM.StopFollowingPlayer();
    }
    
}
