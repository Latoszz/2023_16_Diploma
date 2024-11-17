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
        if (e.isMouse && e.type == EventType.MouseDown) {
            text = e.button switch {
                0 => "Left mouse",
                1 => "Right mouse",
                _ => "None"
            };
        }
        Rect labelRect = new Rect(Screen.width - 700 - 10, 10, 700, 150);
        GUILayout.BeginArea(labelRect);
        GUILayout.Label($"<color='black'><size=100>{text}</size></color>");
        GUILayout.EndArea();
    }
}