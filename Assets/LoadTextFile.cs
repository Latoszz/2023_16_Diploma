using TMPro;
using UnityEngine;

public class LoadTextFile : MonoBehaviour {
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private TMP_Text tmpText;

    private void Awake() {
        tmpText.text = textAsset.text;
    }
}
