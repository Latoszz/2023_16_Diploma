using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;

namespace CardBattles.Mydebug {
    public class TimeSpeedTracker : MonoBehaviour {
        public float time = 0f;
        [SerializeField] private Text text;

        void Start() {
            StartCoroutine(UpdateTimeScale());
        }

        public IEnumerator UpdateTimeScale() {
            while (true) {
                time = Time.timeScale;
                text.text = Math.Round(time,2).ToString();
                yield return new WaitForSeconds(0.2f);
            }
        }


    }
}
