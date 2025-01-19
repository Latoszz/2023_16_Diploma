using System;
using CardBattles.Managers;
using JetBrains.Annotations;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Audio {
    public class AudioManager : MonoBehaviour {
    
        public static AudioManager Instance;
    
        [HorizontalLine, Header("Play On Command")]
        [SerializeField]
        private float lowPitchRange = .85f;
        [SerializeField]
        private float highPitchRange = 1.15f;
    
        [SerializeField]
        public AudioSource effectsSource;
        [SerializeField]
        public AudioSource musicSource;


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Play([CanBeNull] AudioClip clip,float volume=1f, float pitch =1f) 
        {
            if (clip is null) {
                Debug.Log("Tried to play a non existant sound");
                return;
            }
            if (TurnManager.Instance.gameHasEnded && clip.name.StartsWith("End.")) {
                Debug.Log("TurnManager.Instance.gameHasEnded so no more audio");
                return;
            }

            
            effectsSource.volume = volume;
            effectsSource.pitch = pitch;
            effectsSource.PlayOneShot(clip, volume);
        } 

        // ReSharper disable ParameterHidesMember
        public void PlayWithVariation([CanBeNull] AudioClip clip,float volume=1f, float? lowPitchRange = null, float? highPitchRange = null) {
            lowPitchRange ??= this.lowPitchRange;
            highPitchRange ??= this.highPitchRange;
        
            float randomPitch = Random.Range((float)lowPitchRange, (float)highPitchRange);
            Play(clip,volume: volume, pitch: randomPitch);
        }   
        // ReSharper restore ParameterHidesMember


    
        public void PlayRandomFromArray(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            Play(clips[randomIndex]);
        }
        public void PlayRandomFromArrayWithVariation(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            PlayWithVariation(clips[randomIndex]);
        }
    }
}