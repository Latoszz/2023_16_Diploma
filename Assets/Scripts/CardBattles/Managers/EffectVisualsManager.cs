using System;
using System.Collections;
using System.Collections.Generic;
using CardBattles.Enums;
using CardBattles.Particles;
using NaughtyAttributes;
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
                { EffectName.ChangeAttack, ChangeAttackAnimation },
                //{ EffectName.EndOfGame, EndGameEffect }
            };
        }

        [InfoBox("Assign not prefabs, but pull the prefab into the game, and assign that game object")]
        [Required]
        [SerializeField]
        private Transform particlesGameObject;

        [SerializeField] private ParticleParent healVFX;
        [SerializeField] private float healAnimationDuration = 1f;

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator HealVisual(Component target) {
            ParticleFactory(healVFX, target.transform.position);
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

        [SerializeField] private ParticleParent loseVFX;
        [SerializeField] private ParticleParent winVFX;
        [SerializeField] private Transform PlayerHero;
        [SerializeField] private Transform EnemyHero;

        [SerializeField] private float offset = 30f;

        public void EndGameEffectTrigger(bool isItAWin) {
            StartCoroutine(EndGameEffect(isItAWin));
        }
        private IEnumerator EndGameEffect(bool isItAWin) {
            var heroTransform = isItAWin ? EnemyHero : PlayerHero;
            var particle = isItAWin ? winVFX : loseVFX;

            ParticleFactory(particle, heroTransform.position, 0);
            yield return new WaitForSecondsRealtime(0.2f);
            for (int i = 0; i < 8; i++) {
                float angle = i * Mathf.PI * 2f / 8;
                float x = Mathf.Cos(angle) * offset;
                float y = Mathf.Sin(angle) * offset;
                ParticleFactory(particle, heroTransform.position + new Vector3(x, y, 0), 0);
            }
            yield return new WaitForSecondsRealtime(0.2f);
            yield return null;
        }

        private ParticleParent ParticleFactory(ParticleParent vfx, Vector3 position, float duration = 1f) {
           
            var newParticleParent = Instantiate(vfx, particlesGameObject, true);
            newParticleParent.transform.position = position;
            
            StartCoroutine(newParticleParent.PlayFor(healAnimationDuration));
            return newParticleParent;
        }
    }
}