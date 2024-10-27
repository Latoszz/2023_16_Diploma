using System;
using System.Collections;
using System.Collections.Generic;
using CardBattles.Enums;
using CardBattles.Particles;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

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
                { EffectName.ChangeAttack, ChangeAttackAnimation },
                { EffectName.EndOfGame, EndGameEffect }
            };
        }

        [InfoBox("Assign not prefabs, but pull the prefab into the game, and assign that game object")]
        [Required]
        [SerializeField]
        private Transform particlesParent;

        [SerializeField] private ParticleParent healVFX;
        [SerializeField] private float healAnimationDuration = 1f;

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator HealVisual(Component target) {
            var newParticleParent = Instantiate(healVFX, particlesParent, true);
            newParticleParent.transform.position = target.transform.position;

            StartCoroutine(newParticleParent.PlayFor(healAnimationDuration));
            yield return null;
        }

        private IEnumerator DamageVisual(Component target) {
            yield return null;
        }

        private IEnumerator ChangeAttackAnimation(Component target) {
            yield return null;
        }


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void HealVisual() {
            var x = this;
            StartCoroutine(HealVisual(x));
        }

        [SerializeField] private ParticleParent purpleFogVFX;
        [SerializeField] private ParticleParent purplefogSmallVFX;

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private IEnumerator EndGameEffect() {
            StartCoroutine(EndGameEffect(new Component()));
            yield return null;
        }

        private IEnumerator EndGameEffect(Component target) {
            
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}