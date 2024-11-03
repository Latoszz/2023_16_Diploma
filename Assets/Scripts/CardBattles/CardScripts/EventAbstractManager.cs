using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardBattles.CardScripts {
    //generalize it
    public abstract class EventAbstractManager : MonoBehaviour {
        [SerializeField] private static UnityEvent eventToHandle;
        
        private void OnEnable() {
            eventToHandle.AddListener(EventToHandle);
        }

        protected abstract void EventToHandle();

        private void OnDisable() {
            eventToHandle.RemoveListener(EventToHandle);
        }
    }
}