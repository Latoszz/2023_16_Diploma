using System;
using System.Collections;
using System.Collections.Generic;
using CardBattles.Enums;
using UnityEngine;

namespace CardBattles.Managers {
    public class EffectVisualsManager : MonoBehaviour {
        public static EffectVisualsManager Instance;

        private void Awake() {
            if (Instance is null) {
                Instance = this;
                InitializeVisualDictionary();
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(gameObject);
            }
        }

        public delegate IEnumerator EffectAnimationDelegate(Component target);

        public Dictionary<EffectName, EffectAnimationDelegate> visual;

        private void InitializeVisualDictionary() {
            visual = new Dictionary<EffectName, EffectAnimationDelegate> {
                { EffectName.Heal, HealVisual },
                { EffectName.DealDamage, DamageVisual },
                { EffectName.ChangeAttack, ChangeAttackAnimation }
            };
        }


        [SerializeField] private GameObject healVFX;
        
        private IEnumerator HealVisual(Component target) {
            var x = Instantiate(healVFX);//.transform.SetParent(target.transform);
            x.transform.position = target.transform.position;
            yield return new WaitForSeconds(10f);
            Destroy(x);
            yield return null;
        }

        private IEnumerator DamageVisual(Component target) {
            yield return null;
        }

        private IEnumerator ChangeAttackAnimation(Component target) {
            yield return null;
        }
    }
}