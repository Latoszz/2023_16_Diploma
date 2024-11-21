using UnityEngine;

namespace Interaction.Scene {
    public class SceneTrigger : MonoBehaviour {
        [SerializeField] private string loadName;
        [SerializeField] private string unloadName;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                if (loadName != "")
                    SceneSwitcher.Instance.LoadScene(loadName);
                if(unloadName != "")
                    SceneSwitcher.Instance.UnloadScene(unloadName);
            }
        }
    }
}
