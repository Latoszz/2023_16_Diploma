using Events;
using UI.Inventory.Items;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Interaction.Objects {
    public class Book: MonoBehaviour, ICollectible, IPointerClickHandler {
        [Header("Sound")]
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private AudioMixer audioMixer;
        public void Collect() {
            gameObject.SetActive(false);
            GameEventsManager.Instance.ItemEvents.ItemWithIdCollected(name);
        }
        
        private float GetMasterVolume(){
            float value;
            bool result =  audioMixer.GetFloat("SFXVolume", out value);
            if(result){
                return Mathf.Pow(10, value/20);
            }
            return 0f;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, GetMasterVolume());
            Collect();
        }
    }
}
