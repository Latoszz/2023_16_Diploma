    using System.Collections;
using System.Reflection.Emit;
using Audio;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardBattles.Character.Hero {
    public class HeroDisplay : MonoBehaviour {
        [Foldout("Objects"), SerializeField] private Image heroImage;
        [Foldout("Objects"), SerializeField] private Text hpText;

        [Foldout("Text"), SerializeField] private Color fullHpColor = Color.black;
        [Foldout("Text"), SerializeField] private Color missingHpColor = new Color(90, 30, 30);


        [Foldout("Hit Animation"), Label("Duration"), SerializeField]
        private float hitAnimationDuration;

        [Foldout("Hit Animation"), Label("Shake Strength"), SerializeField]
        private float hitAnimationShakeStrength;

        [Foldout("Hit Animation"), Label("Ease"), SerializeField]
        private float hitAnimationKnockbackStrength;
        
        private Hero hero;

        private int currentHealth;

        private void Awake() {
            hero = GetComponent<Hero>();
            hero.currentHealthAction += SetCurrentHealth;
            hero.takeDamageAction += GetHit;
            SetCurrentHealth(hero.MaxHealth);
        }
       [Button(text:"test",enabledMode: EButtonEnableMode.Always)]
        private void Test() {
            var x = hpText.text;
            Debug.Log(x + " " + hero.currentHealth);
            hpText.text = x;
        }

        private void SetCurrentHealth(int value) {
            SetHpColor();
            
            hpText.text = value.ToString();
        }

        private void SetHpColor() {
            return;
            //TODO
            
            /*
            if (hero.HasFullHp)
                hpText.color = fullHpColor;
            else {
                hpText.color = missingHpColor;
            }*/
        }

        [SerializeField] private string takeDamageSound = "Damage.Hero";
        private void GetHit() {
            SetCurrentHealth(hero.currentHealth);
            StartCoroutine(GetHitAnimation());
            var x =AudioCollection.Instance.GetClip(takeDamageSound);
            AudioManager.Instance.PlayWithVariation(x);
        }

        private IEnumerator GetHitAnimation() {
            yield return null;
        }
    }
}