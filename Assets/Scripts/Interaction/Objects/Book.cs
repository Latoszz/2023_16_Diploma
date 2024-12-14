using Events;
using UI.HUD;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Book: MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject bookNote;
        [SerializeField] private GameObject cardSet;
        [SerializeField] private GameObject particleEffect;
        
        [Header("Sound")]
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private AudioMixer audioMixer;
        
        private float GetMasterVolume(){
            float value;
            bool result =  audioMixer.GetFloat("SFXVolume", out value);
            if(result){
                return Mathf.Pow(10, value/20);
            }
            return 0f;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            Open();
        }

        private void Open() {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, GetMasterVolume());
            GameEventsManager.Instance.ItemEvents.ItemWithIdCollected(name);
            HUDController.Instance.HideHUD();
            bookNote.SetActive(true);
            particleEffect.SetActive(false);
        }

        public void Close() {
            bookNote.SetActive(false);
            HUDController.Instance.ShowHUD();
            if (cardSet is not null) {
                cardSet.SetActive(true);
                cardSet = null;
            }
        }
    }
}
