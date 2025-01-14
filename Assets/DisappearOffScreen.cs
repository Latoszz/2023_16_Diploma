using NPCScripts;
using UnityEngine;

public class DisappearOffScreen : MonoBehaviour {
    [SerializeField] private TalkableNPC npc;

    private void OnBecameInvisible() {
        Debug.Log(npc.TalkedTo());
        if (npc.TalkedTo()) {
            gameObject.SetActive(false);
        }
    }
}
