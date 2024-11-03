    using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
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
                { EffectName.ChangeAttack, ChangeAttackVisual },
                { EffectName.BuffHp, BuffHpVisual }
            };
        }


        [InfoBox("Assign not prefabs, but pull the prefab into the game, and assign that game object")]
        [Required]
        [SerializeField]
        private Transform particlesGameObject;

        private ParticleParent ParticleFactory(ParticleParent vfx, Vector3 position, float duration = 1f) {
            var newParticleParent = Instantiate(vfx, particlesGameObject, true);
            newParticleParent.transform.position = position;

            StartCoroutine(newParticleParent.PlayFor(duration));
            return newParticleParent;
        }

        [SerializeField] private ParticleParent healVFX;
        [SerializeField] private float healAnimationDuration = 1f;

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator HealVisual(Component target) {
            ParticleFactory(healVFX, target.transform.position, healAnimationDuration);
            yield return null;
        }

        [SerializeField] private ParticleParent damageVFX;

        private IEnumerator DamageVisual(Component target) {
            ParticleFactory(damageVFX, target.transform.position);
            yield return null;
        }

        private IEnumerator ChangeAttackVisual(Component target) {
            yield return null;
        }

        private IEnumerator BuffHpVisual(Component target) {
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

            ParticleFactory(particle, heroTransform.position, 30);
            yield return new WaitForSecondsRealtime(0.4f);
            for (int i = 0; i < 8; i++) {
                float angle = i * Mathf.PI * 2f / 8;
                float x = Mathf.Cos(angle) * offset;
                float y = Mathf.Sin(angle) * offset;
                ParticleFactory(particle, heroTransform.position + new Vector3(x, y, 0), 30);
            }

            yield return new WaitForSecondsRealtime(0.2f);
            yield return null;
        }


        [SerializeField] private ParticleParent explosionVFX;
        [SerializeField] private string explosionSoundName = "Boom";

        public ParticleParent Explosion(Vector3 position, float duration = 1f) {
            var x = AudioCollection.Instance.GetClip(explosionSoundName);
            AudioManager.Instance.PlayWithVariation(x);
            return ParticleFactory(explosionVFX, position, duration);
        }
    }
}