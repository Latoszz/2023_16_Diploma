using System.Collections;
using System.Linq;
using CardBattles.Character;
using CardBattles.Character.Mana;
using CardBattles.Managers;
using DG.Tweening;
using UnityEngine;

namespace CardBattles {
    public class EndTurnOutlineFlash : MonoBehaviour {
        [SerializeField] private CanvasGroup outlineCanvasGroup;
        private CharacterManager character;

        private void Awake() {
            character = GetComponent<CharacterManager>();
        }

        private void OnEnable() {
            ManaManager.noManaLeft.AddListener(TryShowOutline);
        }

        private void OnDisable() {
            ManaManager.noManaLeft.RemoveListener(TryShowOutline);
        }

        private void TryShowOutline() {
            var currentMana = character.manaManager.CurrentMana;
            var lowestCostCard = character.hand.Cards.Select(x => x.GetCost()).ToList().Min();
            if (currentMana < lowestCostCard)
                StartCoroutine(ShowOutline());
        }

        private IEnumerator ShowOutline() {
            if (TurnManager.Instance.isPlayersTurn)
                StartCoroutine(OutlineVisual(true));

            yield return new WaitUntil(() => !TurnManager.Instance.isPlayersTurn);
            StartCoroutine(OutlineVisual(false));
        }

        [SerializeField] private float outlineDuration = 0.33f;
        private IEnumerator OutlineVisual(bool val) {
            var x = val ? 1 : 0;

            var tween = outlineCanvasGroup
                .DOFade(x, outlineDuration)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            
            tween.Play();
            
            yield return tween.WaitForCompletion();


        }
    }
}
