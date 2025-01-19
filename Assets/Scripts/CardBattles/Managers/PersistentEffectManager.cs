using System.Collections;
using System.Collections.Generic;
using CardBattles.Enums;
using UnityEngine;

namespace CardBattles.Managers {
    public class EffectData {
        public List<GameObject> Targets;
        public string EffectName;
        public float Value;
    }
    public class PersistentEffectManager : MonoBehaviour {
        public static PersistentEffectManager Instance;
        private Queue<EffectData> effectQueue = new Queue<EffectData>();
        private bool isProcessingQueue = false;


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
        }
        public void DoEffect(List<GameObject> targets, EffectName effectName, int value)
        {
            StartCoroutine(DoEffectCoroutine(targets, effectName, value));
        }

        private IEnumerator DoEffectCoroutine(List<GameObject> targets, EffectName effectName, int value)
        {
            yield return StartCoroutine(
                EffectManager.effectDictionary[effectName](targets, value)
            );
        }
    }
}