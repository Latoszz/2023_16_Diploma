using System.Collections.Generic;
using UnityEngine;

namespace Audio {
     public class PlayerSound : MonoBehaviour {
          [SerializeField] private float minVolume = 0.1f;
          [SerializeField] private float maxVolume = 0.3f;
          [SerializeField] private float minPitch = 0.8f;
          [SerializeField] private float maxPitch = 1.2f;
          
          public List<AudioClip> footstepSounds;
          private AudioSource audioSource;

          private void Awake() {
               audioSource = GetComponent<AudioSource>();
          }

          public void PlayFootstep() {
               AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
               audioSource.clip = clip;
               audioSource.volume = Random.Range(minVolume, maxVolume);
               audioSource.pitch = Random.Range(minPitch, maxPitch);
               audioSource.Play();
          }
     }
}
