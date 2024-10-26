using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.ForEditor {
    public class TimeScaler : ForEditorComponent {
        [SerializeField] [OnValueChanged("UpdateTimeCallback")] [Range(0, 2)]
        private float timeScale = 1f;

        [Button]
        private void ReturnTo1() {
            timeScale = 1f;
        }
        public void UpdateTimeCallback() {
            if(!enabled)
                return;
            Time.timeScale = timeScale;
        }

    }
}
