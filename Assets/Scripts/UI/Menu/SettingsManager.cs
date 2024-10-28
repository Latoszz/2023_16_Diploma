using System.Collections.Generic;
using Esper.ESave;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour, ISavable {
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private GameObject audioVideoPanel;

    private float currentMusicVolume = 0.5f;
    private float currentSFXVolume = 0.5f;
    private List<Resolution> resolutions;
    private static List<string> options;
    private int currentResolutionIndex;
    private bool isFullscreen = true;

    private bool isResolutionLoaded = false;

    private const string MusicVolumeSaveID = "MusicVolume";
    private const string SFXVolumeSaveID = "SFXVolume";
    private const string ResolutionSaveID = "ResolutionId";
    private const string FullscreenSaveID = "FullscreenId";
    
    void Start() {
        SetUpResolutions();
    }

    public void SetFullscreen() {
        isFullscreen = fullscreenToggle.isOn;
        Screen.fullScreenMode = fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetResolution() {
        ChangeResolution(resolutionDropdown.value);
    }
    
    public void SetMusicVolume(float volume) {
        currentMusicVolume = volume;
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume(float volume) {
        currentSFXVolume = volume;
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    private void SetUpResolutions() {
        resolutions = new List<Resolution>();
        var availableResolutions = Screen.resolutions;
        options = new List<string>();
        
        for (int i = 0; i < availableResolutions.Length; i++) {
            int width = availableResolutions[i].width;
            int height = availableResolutions[i].height;
            
            if(height < 720)
                continue;
            resolutions.Add(availableResolutions[i]);
            string option = width+ " x " + height;
            options.Add(option);

           // if (!isResolutionLoaded) {
                if (availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height) {
                    currentResolutionIndex = i;
                }
            //}
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void ChangeResolution(int resolutionIndex) {
        currentResolutionIndex = resolutionIndex;
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        Resolution resolution = resolutions[resolutionIndex];
        if(!resolution.Equals(Screen.currentResolution)) 
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

   public void PopulateSaveData(SaveFile saveFile) {
        saveFile.AddOrUpdateData(MusicVolumeSaveID, currentMusicVolume);
        saveFile.AddOrUpdateData(SFXVolumeSaveID, currentSFXVolume);
        saveFile.AddOrUpdateData(ResolutionSaveID, currentResolutionIndex);
        saveFile.AddOrUpdateData(FullscreenSaveID, isFullscreen);
    }

    public void LoadSaveData(SaveFile saveFile) {
        if (saveFile.HasData(MusicVolumeSaveID)) {
            SetMusicVolume(saveFile.GetData<float>(MusicVolumeSaveID));
            musicSlider.value = currentMusicVolume;
            SetSFXVolume(saveFile.GetData<float>(SFXVolumeSaveID));
            sfxSlider.value = currentSFXVolume;
        
            currentResolutionIndex = saveFile.GetData<int>(ResolutionSaveID);
            isResolutionLoaded = true;
            SetUpResolutions();
            ChangeResolution(currentResolutionIndex);
        
            fullscreenToggle.isOn = saveFile.GetData<bool>(FullscreenSaveID);
            SetFullscreen();
        }
    }
}
