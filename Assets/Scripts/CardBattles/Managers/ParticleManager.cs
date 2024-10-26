using UnityEngine;

namespace CardBattles.Managers {
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }

        private void Awake() {
            if (Instance is null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
   
    }
}

