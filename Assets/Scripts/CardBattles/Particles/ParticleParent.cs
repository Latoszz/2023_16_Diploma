using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace CardBattles.Particles {
    public class ParticleParent : MonoBehaviour {
        [SerializeField] private ParticleSystem ps;

        private void Awake() {
            ps = GetComponent<ParticleSystem>();
        }

        private void TurnOffDefaultParticles() {
            this.AddComponent<ParticleSystem>();
            ps = GetComponent<ParticleSystem>();
            var e = ps.emission;
            var s = ps.shape;
            e.enabled = false;
            s.enabled = false;
        }

        public IEnumerator PlayFor(float time = 1f) {
            var localPs = ps;
            float x = 0;
                var tween = DOTween
                    .To(() => x,
                        y => x = y,
                        5f,
                        time);
            
            tween.OnPlay(() => { localPs.Play(true); });
            tween.OnComplete(() => {
                    localPs.Stop();
                    StartCoroutine(KillWhenDone());
                    
                }).SetLink(this.gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion();

            tween.Play();
            yield return tween;
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }


        private IEnumerator KillWhenDone() {
            StartCoroutine(SanityCheck());
            yield return new WaitUntil(() => !ps.IsAlive(true));
            Destroy(gameObject);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator SanityCheck() {
            yield return new WaitForSeconds(30f);
            Debug.LogError("WAWOWAAWOO THIS PARTICLE HAS BEEN ALIVE FOR OVER 30 SECONDS AFTER STOPPING");
        }

        [Button]
        private void SetupChildrenButton() {
            if (!TryGetComponent(typeof(ParticleSystem), out var _))
                TurnOffDefaultParticles();
            
            transform.localScale = Vector3.one * 20;
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var system in particleSystems) {
                var main = system.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;

                var component = system.GetComponent<Renderer>();
                component.sortingLayerName = "VFX";
            }
        }
    }
}