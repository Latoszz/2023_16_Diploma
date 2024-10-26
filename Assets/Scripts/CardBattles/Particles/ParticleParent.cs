using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace CardBattles.Particles {
	public class ParticleParent : MonoBehaviour {

		private ParticleSystem ps;

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

	
		public void PlayVFX() {
			ps.Play(true);
		}

		public IEnumerator PlayFor(float time = 1f) {
			ps.Play(true);
			yield return new WaitForSeconds(time);
			ps.Stop(true);
			Debug.Log($"{gameObject.name} active state: {gameObject.activeInHierarchy}");

			StartCoroutine(KillWhenDone());
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

		[Button(enabledMode: EButtonEnableMode.Playmode)]
		private void PlayForButton() {
			StartCoroutine(PlayFor(1f));
		}

		[Button]
		private void SetupChildrenButton() {
			if(!TryGetComponent(typeof(ParticleSystem),out var _))
				TurnOffDefaultParticles();

				
			transform.localScale = Vector3.one*20;
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
