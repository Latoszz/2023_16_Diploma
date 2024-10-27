using UnityEngine;

namespace ScriptableObjects.Dialogue {
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue Text")]
    public class DialogueText : ScriptableObject {
        [SerializeField] private Sprite icon;
        [SerializeField] private string nameText;
        [TextArea(5, 10)]
        [SerializeField] private string[] sentences;
        
        public Sprite Icon => icon;
        public string NameText => nameText;
        public string[] Sentences => sentences;
    }
}