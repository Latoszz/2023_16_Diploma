using System.Collections;
using System.Collections.Generic;
using CardBattles.Interfaces.InterfaceObjects;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;


//TODO hate this script, someone please rework it, its awful, thankfully this isn't important code but :(
namespace CardBattles.Character.Mana.Additional {
    public class ManaDisplay : PlayerEnemyMonoBehaviour {
        [SerializeField] public GameObject manaPointPrefab;

        [SerializeField] private float distanceBetweenManaPoints = 30f;
        [SerializeField] private List<ManaPoint> manaPoints;

        [SerializeField] private Transform firstPointTransform;

        [Space] [BoxGroup("No mana animation"), Label("Shake Strength"), SerializeField]
        private int noManaShakeStrength = 15;

        [BoxGroup("No mana animation"), Label("Time"), SerializeField]
        private float shakeTime = 0.3f;

        [BoxGroup("No mana animation"), Label("Ease"), SerializeField]
        private Ease noManaAnimationEase;


        [BoxGroup("Refresh Animation"), SerializeField, Label("Flash Color")]
        private Color refreshAnimationFlashColor = Color.white;

        /*[Space] [BoxGroup("Refresh Animation"), SerializeField, Label("Duration")]
        private float refreshAnimationDuration = 0.8f;

        [BoxGroup("Refresh Animation"), SerializeField, Label("% Duration of ease in"), Range(0, 1)]
        private float refreshAnimationEasePercentage = 0.5f;

        [Space] [BoxGroup("Refresh Animation"), SerializeField, Label("In Ease")]
        private Ease refreshAnimationInEase = Ease.InCubic;

        [BoxGroup("Refresh Animation"), SerializeField, Label("Out Ease")]
        private Ease refreshAnimationOutEase = Ease.OutCubic;*/


        private int currentMana; // backing field

        public int CurrentMana {
            get => currentMana;
            set {
                if (currentMana != value) {
                    currentMana = value;
                    OnCurrentManaChanged(value);
                }
            }
        }


        public void UpdateMaxMana(int value) {
            SetMaxManaPointAmount(value);

            //TODO NOT SURE IF THIS IS NEEDED
            OnCurrentManaChanged(currentMana);
        }

        private void SetMaxManaPointAmount(int value) {
            if (value == manaPoints.Count)
                return;
            var firstPosition = firstPointTransform.position;
            var positions = new List<Vector3>();
            for (int i = 0; i < value; i++) {
                var pos = firstPosition;
                pos.x += distanceBetweenManaPoints * i;
                positions.Add(pos);
            }

            //TODO it seems so wrong to do it this way, dont have brain energy to do it smarter
            manaPoints.ForEach(Destroy);
            manaPoints = new List<ManaPoint>();

            foreach (var pos in positions) {
                var manaPoint = Instantiate(manaPointPrefab, transform);
                manaPoints.Add(manaPoint.GetComponent<ManaPoint>());
                manaPoint.transform.position = pos;
                if (!IsPlayers)
                    manaPoint.transform.rotation *= quaternion.Euler(math.PI, 0, 0);
            }
            //
        }

        private void OnCurrentManaChanged(int value) {
            if (value > manaPoints.Count) {
                Debug.LogError("shouldn't reach this code, if it does, fix the code");
            }

            for (int i = 0; i < manaPoints.Count; i++) {
                manaPoints[i].SetState(i < value);
            }
        }

        public IEnumerator ShowLackOfMana() {
            var sequence = DOTween.Sequence();
            var boxShake = transform
                .DOShakePosition(
                    shakeTime,
                    new Vector3(noManaShakeStrength, 0, 0),
                    randomnessMode: ShakeRandomnessMode.Harmonic)
                .SetLoops(3, LoopType.Yoyo);

            sequence.Append(boxShake);

            foreach (var manaPoint in manaPoints) {
                foreach (var image in manaPoint.Images) {
                    var manaPointColorSwitch = image
                        .DOColor(Color.red, shakeTime)
                        .SetEase(noManaAnimationEase)
                        .SetLoops(4, LoopType.Yoyo);
                    sequence.Join(manaPointColorSwitch);
                }
            }

            yield return sequence.Play();
        }

        /*
        private IEnumerator MakeAvailable(ManaPoint manaPoint, bool skipAnimation = false) {
            if (manaPoint.isAvailable && !skipAnimation) {
                yield return MakeAvailable(manaPoint, true);
            }

            var changeSequence = DOTween.Sequence();

            var duration = refreshAnimationDuration;
            if (skipAnimation)
                duration = 0f;


            /*var flashTween = manaPoint.image
                .DOColor(
                    refreshAnimationFlashColor,
                    duration * refreshAnimationEasePercentage)
                .SetEase(refreshAnimationInEase);
            var colorToAvailable = manaPoint.image
                .DOColor(
                    isAvailableColor,
                    duration * (1 - refreshAnimationEasePercentage))
                .SetEase(refreshAnimationOutEase);

            changeSequence.Join(flashTween);
            changeSequence.Append(colorToAvailable);
            #1#

            yield return changeSequence;
        }
    }*/
    }
}