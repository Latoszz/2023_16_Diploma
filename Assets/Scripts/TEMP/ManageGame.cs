using System.Collections.Generic;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour {
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EnemyStateManager enemyStateManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject npc2;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject wall;
    [SerializeField] private ParticleSystem removablePS;
    [SerializeField] private List<CardSetItem> cardSets;
    
    public static ManageGame Instance = null;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        
        if (SaveManager.Instance.HasInventoryData())
            return;
        
        foreach (CardSetItem cardSet in cardSets) {
            InventoryController.Instance.AddItem(cardSet);
        }
        
    }

}
