using System;
using UnityEngine;

public class EnemySM : StateMachine {
    [HideInInspector]
    public LockedState lockedState;
    [HideInInspector]
    public UndefeatedState undefeatedState;
    [HideInInspector]
    public DefeatedState defeatedState;
    
    [SerializeField] private Enemy enemy;
    private EnemyState state;

    [SerializeField] private string enemyID;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        enemyID = Guid.NewGuid().ToString();
    }

    [SerializeField] private Material undefeatedMaterial;
    [SerializeField] private Material defeatedMaterial;

    private bool isLocked, isBeaten;
    private Renderer objectRenderer;
    
    private void Awake() {
        objectRenderer = GetComponent<Renderer>();
        
        lockedState = new LockedState(this);
        undefeatedState = new UndefeatedState(this);
        defeatedState = new DefeatedState(this);
    }

    protected override BaseState GetInitialState() {
        return lockedState;
    }

    public void ChangeState(EnemyState state) {
        this.state = state;
    }

    public bool IsLocked() {
        return isLocked;
    }

    public void SetLocked(bool value) {
        isLocked = value;
    }
    
    public bool IsBeaten() {
        return isBeaten;
    }

    public void SetBeaten(bool value) {
        isBeaten = value;
    }

    public Enemy GetEnemy() {
        return enemy;
    }
    
    public EnemyState GetState() {
        return state;
    }

    public Material GetUndefeatedMaterial() {
        return undefeatedMaterial;
    }
    
    public Material GetDefeatedMaterial() {
        return defeatedMaterial;
    }

    public Renderer GetRenderer() {
        return objectRenderer;
    }

    public string GetID() {
        return enemyID;
    }
}
