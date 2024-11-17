using System;
using System.Collections;
using CardBattles.Enums;
using CardBattles.Interfaces;
using CardBattles.Interfaces.InterfaceObjects;
using CardBattles.Managers;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.CardScripts.Additional {
    public class CardAnimation : PlayerEnemyMonoBehaviour {
        [Header("Show")] [Foldout("Draw"), SerializeField]
        public Vector3 showPosition;

        [Foldout("Draw"), SerializeField]
        public Vector3 showScale = Vector3.one;


        [Space, Header("Deck to show")] [Foldout("Draw"), SerializeField]
        private Ease playerToShow;

        [Foldout("Draw"), Range(0, 1), SerializeField]
        private float timeToShow = 0.5f;


        [Space, Header("Show to hand")] [Foldout("Draw"), SerializeField]
        private Ease showToHand;

        [Foldout("Draw"), Range(0, 1), SerializeField]
        private float timeToHand = 0.5f;


        [Space, Header("Enemy Draw")] [Foldout("Draw"), SerializeField]
        private Ease enemyToHand;

        [Foldout("Draw"), Range(0, 1), SerializeField]
        private float enemyDraw = 0.5f;

        private Card card;
        private void Awake() {
            card = GetComponent<Card>();
        }

        public IEnumerator DrawAnimation(Vector3 finalPosition) {
            var drawAnimation = StartCoroutine(IsPlayers
                ? PlayerDrawAnimationCoroutine(finalPosition)
                : EnemyDrawAnimationCoroutine(finalPosition));
            yield return drawAnimation;
        }

        private IEnumerator PlayerDrawAnimationCoroutine(Vector3 finalPosition) {
            var sequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            StartCoroutine(card.ChangeSortingOrderTemporarily(30, false));
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            sequence
                .Append(transform
                    .DOMove(showPosition, timeToShow)
                    .SetEase(playerToShow))
                .Join(transform
                    .DOScale(showScale, timeToShow)
                    .SetEase(playerToShow));

            sequence
                .Append(transform
                    .DOMove(finalPosition, timeToHand)
                    .SetEase(showToHand))
                .Join(transform
                    .DOScale(Vector3.one * CardDisplay.scaleInHand, timeToHand)
                    .SetEase(showToHand));

            sequence.Play();
            yield return sequence.WaitForCompletion();
            StartCoroutine(card.ChangeSortingOrderTemporarily(-30, false));

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator EnemyDrawAnimationCoroutine(Vector3 finalPosition) {
            var sequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            sequence.Append(
                transform
                    .DOMove(finalPosition, enemyDraw)
                    .SetEase(enemyToHand));
            sequence.Join(
                transform
                    .DOScale(Vector3.one * CardDisplay.scaleInHand
                        , enemyDraw));
            sequence.Play();
            yield return sequence.WaitForCompletion();
        }


        [Foldout("Move To")] [SerializeField]
        private float moveToAnimationTime = 0.3f;

        [Foldout("Move To")] [SerializeField]
        private Ease moveToEase;


        public IEnumerator MoveTo(Vector3 vector3) {
            yield return transform
                .DOMove(vector3, moveToAnimationTime)
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion(true);
        }

        [Foldout("Play Card")] [InfoBox("Currently only used when enemy plays a card")] [SerializeField]
        private float playCardTime = 0.4f;

        [Foldout("Play Card")] [SerializeField]
        private Ease playCardEase = Ease.InOutSine;

        public IEnumerator PlayToCardSpotAnimation(CardSpot cardSpot) {
            yield return transform
                .DOMove(
                    cardSpot.transform.position,
                    playCardTime)
                .SetEase(playCardEase)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion();
            CardSpot.PlayDropSound();
        }


        [Space, Header("MoveTo")] [Foldout("Attack Animation"), SerializeField]
        private float attackMoveToTime;

        [Foldout("Attack Animation"), SerializeField]
        private Ease attackMoveToEase;

        [Space, Header("KnockBack")] [Foldout("Attack Animation"), SerializeField]
        private float attackKnockBackAmount;

        [Foldout("Attack Animation"), SerializeField]
        private float attackKnockBackShakeStrength;

        [Foldout("Attack Animation"), SerializeField]
        private float attackKnockBackTime;

        [Foldout("Attack Animation"), SerializeField]
        private Ease attackKnockBackEase;

        [Space, Header("MoveBack")] [Foldout("Attack Animation"), SerializeField]
        private float attackMoveBackTime;

        [Foldout("Attack Animation"), SerializeField]
        private Ease attackMoveBackEase;

        //TODO Add distance scaling to animation
        //USE LIKE A STATIC METHOD 
        public IEnumerator AttackAnimation(IAttacker attacker, IDamageable damageable) {
            bool poisonous = false;
            if (attacker is Minion minion) {
                if (minion.Properties.Contains(AdditionalProperty.Poisonous)) {
                    poisonous = true;
                }
            }

            var attack = attacker.GetAttack();
            var attackerTransform = attacker.GetTransform();
            var originalPosition = attackerTransform.position;
            var targetPosition = damageable.GetTransform().position;
            var moveDirection = (targetPosition - originalPosition).normalized;
            yield return AttackMoveToTarget(attackerTransform, targetPosition);

            damageable.TakeDamage(attack, poisonous);

            //TODO add variable to determine how much time to stop for at a target
            yield return new WaitForEndOfFrame();
            yield return AttackKnockback(attackerTransform, moveDirection);

            yield return AttackMoveBack(attackerTransform, originalPosition);

            yield return null;
        }

        private IEnumerator AttackMoveToTarget(Transform attackerTransform, Vector3 finalPosition) {
            var moveToTarget = attackerTransform
                .DOMove(
                    finalPosition,
                    attackMoveToTime)
                .SetEase(attackMoveToEase)
                .SetLink(attackerTransform.gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion(true);
            yield return moveToTarget;
        }

        private IEnumerator AttackKnockback(Transform attackerTransform, Vector3 moveDirection) {
            //TODO for some reason this doesnt animate both shake and move at once
            //var move = StartCoroutine(AttackKnockbackMove(attackerTransform, moveDirection));
            //var shake = StartCoroutine(AttackKnockbackShake(attackerTransform));
            //TODO so i had to do this
            var sequence = DOTween.Sequence().SetLink(attackerTransform.gameObject, LinkBehaviour.KillOnDestroy);

            var knockbackPosition = attackerTransform.position - (moveDirection * attackKnockBackAmount);

            sequence.Join(attackerTransform
                .DOMove(
                    knockbackPosition,
                    attackKnockBackTime)
                .SetEase(attackKnockBackEase));

            sequence.Play();
            yield return sequence.WaitForCompletion();
        }

        private IEnumerator AttackKnockbackMove(Transform attackerTransform, Vector3 moveDirection) {
            var knockbackPosition = attackerTransform.position - moveDirection * attackKnockBackAmount;

            var knockBack = attackerTransform
                .DOMove(
                    knockbackPosition,
                    attackKnockBackTime)
                .SetEase(attackKnockBackEase)
                .SetLink(attackerTransform.gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion(true);
            yield return knockBack;
        }

        private IEnumerator AttackKnockbackShake(Transform attackerTransform) {
            var shake = attackerTransform
                .DOShakePosition(
                    attackKnockBackTime,
                    attackKnockBackShakeStrength)
                .SetEase(attackMoveToEase)
                .SetLink(attackerTransform.gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion(true);
            yield return shake;
        }

        private IEnumerator AttackMoveBack(Transform attackerTransform, Vector3 originalPosition) {
            var moveBack = attackerTransform
                .DOMove(
                    originalPosition,
                    attackMoveBackTime)
                .SetEase(attackMoveBackEase)
                .SetLink(attackerTransform.gameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion(true);
            yield return moveBack;
        }

        //check if the card is dragged
        private IEnumerator CanBeAnimated() {
            throw new NotImplementedException();
        }

        public IEnumerator Die() {
            yield return new WaitForSeconds(0.4f);
            EffectVisualsManager.Instance.Explosion(transform.position, 3);

            yield return null;
        }

        public IEnumerator TakeDamage() {
            yield return null;
        }

        public IEnumerator Play(Card card) {
            var cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.ChangeCardVisible(true);
            switch (card) {
                case Spell:
                    if (!card.IsPlayers)
                        yield return StartCoroutine(MoveToMiddle(card));
                    StartCoroutine(card.DoEffect(EffectTrigger.OnPlay));
                    StartCoroutine(SpellShowcase(card.gameObject));
                    yield return StartCoroutine(FadeOut(card.gameObject));
                    break;
            }

            yield return null;
        }

        private IEnumerator MoveToMiddle(Card card) {
            StartCoroutine(card.ChangeSortingOrderTemporarily(15));
            var cor = card.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic)
                .SetLink(card.gameObject, LinkBehaviour.KillOnDestroy);
            yield return cor.WaitForCompletion();
        }

        [Space, Header("FadeOut"), Foldout("FadeOut"), SerializeField]
        private float cardFadeOutDuration = 0.7f;

        [Foldout("FadeOut"), SerializeField]
        private Ease cardFadeOutEase = Ease.InOutQuad;

        private IEnumerator FadeOut(GameObject cardGameObject) {
            if (!TryGetComponent(typeof(CanvasGroup), out var canvasGroup))
                yield break;

            StartCoroutine(Spin(cardGameObject));
            yield return ((CanvasGroup)canvasGroup)
                .DOFade(0f, cardFadeOutDuration)
                .SetEase(cardFadeOutEase)
                .SetLink(cardGameObject, LinkBehaviour.KillOnDestroy)
                .WaitForCompletion();
        }

        private IEnumerator Spin(GameObject cardGameObject) {
            var cor = transform
                .DORotate(new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(Ease.Linear).SetLoops(-1)
                .SetLink(cardGameObject, LinkBehaviour.KillOnDestroy);
            cor.Play();
            yield return new WaitForSeconds(cardFadeOutDuration);
        }

        [SerializeField, Foldout("SpellShowcase"), Label("Position")]
        private Vector3 spellShowcasePosition;

        [SerializeField, Foldout("SpellShowcase"), Label("Start Scale")]
        private float startScaleSpellShowcase;

        [SerializeField, Foldout("SpellShowcase"), Label("Max Scale")]
        private float maxScaleSpellShowcase;

        [SerializeField, Foldout("SpellShowcase"), Label("Grow Time")]
        private float growTimeSpellShowcase;

        [SerializeField, Foldout("SpellShowcase"), Label("Stay ForTime")]
        private float stayForTimeSpellShowcase;

        [SerializeField, Foldout("SpellShowcase"), Label("Shrink Time")]
        private float shrinkTimeSpellShowcase;
        
    
        private IEnumerator SpellShowcase(GameObject spellGameObject) {
            var x = Instantiate(spellGameObject, new Vector3(4000, 0, 0), Quaternion.identity,BoardManager.Instance.transform).transform;
            

            x.position = spellShowcasePosition;
            var canvasGroup = x.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            x.localScale = Vector3.one * startScaleSpellShowcase;

            var sequence = DOTween.Sequence().SetLink(x.gameObject, LinkBehaviour.KillOnDestroy);

            sequence.Append(x.DOScale(maxScaleSpellShowcase, growTimeSpellShowcase).SetEase(Ease.OutElastic));

            sequence.AppendInterval(stayForTimeSpellShowcase);

            sequence.Append(x.DORotate(new Vector3(0, 360, 0), shrinkTimeSpellShowcase, RotateMode.FastBeyond360)
                .SetEase(Ease.InCubic));

            sequence.Join(x.DOScale(0, shrinkTimeSpellShowcase).SetEase(Ease.OutCubic));
            sequence.Join(x.GetComponent<CanvasGroup>().DOFade(0, shrinkTimeSpellShowcase));
            var canvas = x.GetComponent<Canvas>();

            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            canvas.sortingLayerName = "Above all";

            // Destroy the object after the sequence
            sequence.OnComplete(() => Destroy(x.gameObject));

            sequence.Play();

            yield return sequence.WaitForCompletion();

        }

        [SerializeField] private float onPlayEffectDelay = 0.16f;

        public IEnumerator OnPlayEffectDelay() {
            yield return new WaitForSeconds(onPlayEffectDelay);
        }
    }
}