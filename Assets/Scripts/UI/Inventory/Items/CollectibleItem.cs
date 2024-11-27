using System;
using Events;
using NPCScripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace UI.Inventory.Items {
    public class CollectibleItem : Item, ICollectible, IPointerClickHandler {
        [SerializeField] private CollectibleItemData itemData;
        [SerializeField] private TalkableNPC npc;
        [SerializeField] private string itemID;
        [ContextMenu("Generate guid for id")]
        private void GenerateGuid() {
            itemID = Guid.NewGuid().ToString();
        }

        [Header("Sound")]
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private AudioMixer audioMixer;
    
        private bool collected  = false;
        public CollectibleItemData GetItemData() {
            return itemData;
        }
    
        public void SetItemData(CollectibleItemData itemData) {
            this.itemData = itemData;
        }

        public void Collect() {
            InventoryController.Instance.AddItem(this);
            collected = true;
            gameObject.SetActive(false);
            GameEventsManager.Instance.ItemEvents.ItemWithIdCollected(itemName);
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
            if(npc != null)
                npc.SetUpNextDialogue();
        }

        public string GetID() {
            return itemID;
        }

        public bool IsCollected() {
            return collected;
        }

        public void SetCollected(bool value) {
            collected = value;
            gameObject.SetActive(!collected);
        }
    }
}