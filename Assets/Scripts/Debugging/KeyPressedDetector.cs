using UnityEngine;

public class KeyPressedDetector : MonoBehaviour {
    private string text = "None";
    void OnGUI() {
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown) {
            if (e.keyCode != KeyCode.None) {
                text = e.keyCode.ToString();
            }
        }
        GUILayout.Label($"<color='black'><size=100>{text}</size></color>");
    }
}