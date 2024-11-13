using System;
using Audio;
using CardBattles.Character.Mana.Additional;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardBattles.Character.Mana {
    public class ManaManager : MonoBehaviour {
        private ManaDisplay manaDisplay;

        //TODO change
        [ShowNativeProperty] private string Mana => $"{currentMana}/{maxMana}";
        [SerializeField] public int maxMana = 2;

        [Min(0)] private int currentMana;

        private bool isPlayers;

        public int CurrentMana {
            get {
                manaDisplay.CurrentMana = currentMana;
                return currentMana;
            }
            set {
                manaDisplay.CurrentMana = value;
                currentMana = value;
            }
        }

        [SerializeField] private AudioClip spendMana;
        [SerializeField] private AudioClip refreshMana;

        private void Awake() {
            isPlayers = CompareTag("Player");
            manaDisplay = GetComponentInChildren<ManaDisplay>();
            manaDisplay.UpdateMaxMana(maxMana);
            CurrentMana = maxMana;
        }

        public bool CanUseMana(IHasCost cost) {
            return CurrentMana >= cost.GetCost();
        }
        public bool CanUseMana(int cost) {
            return CurrentMana >= cost;
        }

        public void UseMana(IHasCost cost) {
            if (!CanUseMana(cost)) {
                Debug.LogException(
                    new ArgumentException(
                        $"Not enough mana to play a card\ncost: {cost.GetCost()}   mana: {currentMana}"), (Object)cost);
            }

            CurrentMana -= cost.GetCost();
        }



        public bool TryUseMana(IHasCost cost) {
            if (!CanUseMana(cost)) {
                ShowLackOfMana();
                return false;
            }

            UseMana(cost);
            return true;
        }

        public bool TryUseMana(int cost) {
            return TryUseMana(new HasCost(cost));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void RefreshMana() {
            CurrentMana = maxMana;
        }

        private void ShowLackOfMana() {
            if (isPlayers)
                StartCoroutine(manaDisplay.ShowLackOfMana());
        }
    }
}