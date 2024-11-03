using System.Collections.Generic;
using UnityEngine;

namespace Audio {
     public class PlayerSound : MonoBehaviour {
          public List<AudioClip> footstepSounds;
          private AudioSource audioSource;

          private void Awake() {
               audioSource = GetComponent<AudioSource>();
          }

          public void PlayFootstep() {
               AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
               audioSource.clip = clip;
               audioSource.volume = Random.Range(0.1f, 0.3f);
               audioSource.pitch = Random.Range(0.8f, 1.2f);
               audioSource.Play();
          }
     }
}
