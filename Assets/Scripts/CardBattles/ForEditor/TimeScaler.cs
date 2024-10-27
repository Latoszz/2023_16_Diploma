using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattles.ForEditor {
    public class TimeScaler : ForEditorComponent {
        [SerializeField] [OnValueChanged("UpdateTimeCallback")] [Range(0, 2)]
        public float timeScale = 1f;

        [ShowNativeProperty]
        private float TimeScale => Time.timeScale;
    
        [Button]
        private void ReturnTo1() {
            timeScale = 1f;
            UpdateTimeCallback();
        }
        public void UpdateTimeCallbackValue(float val) {
            
            if(!enabled || !Application.isPlaying)
                return;
            timeScale = val;
            UpdateTimeCallback();

        }
        public void UpdateTimeCallback() {
            if(!enabled)
                return;
            Time.timeScale = timeScale;
        }

    }
}
