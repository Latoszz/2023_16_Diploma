using System;
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

        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;

        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip villageMusic;
        [SerializeField] private AudioClip villageAmbience;
        [SerializeField] private AudioClip cardBattleMusic;
    
        [HorizontalLine, Header("Play On Command")]
        [SerializeField]
        private float lowPitchRange = .85f;
        [SerializeField]
        private float highPitchRange = 1.15f;
    
        public AudioSource effectsSource;
        public AudioSource musicSource;


        
        private void Start() {
            SceneManager.activeSceneChanged += ChangeMusic;
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Play([CanBeNull] AudioClip clip,float volume=1f, float pitch =1f) 
        {
            Debug.Log(clip.name);
            if (clip is null) {
                Debug.Log("Tried to play a non existant sound");
                return;
            }
            effectsSource.volume = volume;
            effectsSource.pitch = pitch;
            effectsSource.clip = clip;
            effectsSource.Play();
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
    
        private void Awake() {
            if (Instance != null && Instance != this) {
                if (effectsSource is not null)
                    Instance.effectsSource = effectsSource;
                if (musicSource is not null)
                    Instance.musicSource = musicSource;
                Destroy(gameObject);
            }
            else {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }

        private void ChangeMusic(Scene scene, Scene scene1) {
            if (scene1.name == "Main Menu") {
                musicAudioSource.clip = menuMusic;
                sfxAudioSource.mute = true;
            }
        
            else if (scene1.name is "beta-release" or "beta-release-2") {
                sfxAudioSource.mute = false;
                musicAudioSource.clip = villageMusic;
                sfxAudioSource.clip = villageAmbience;
                musicAudioSource.Play();
                sfxAudioSource.Play();
            }
        }
    }
}