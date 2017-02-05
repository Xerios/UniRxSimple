using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Example4 : MonoBehaviour
{
    public Button damageButton, resetButton;

    public Text hpText;
    public GameObject hpBar, hpDelta;

    // We use IntReactiveProperty so that we can see and modify the value in the Inspector, other than that it's the same as ReactiveProperty<Int>
    public IntReactiveProperty hp = new IntReactiveProperty(100);

    // Use this for initialization
    void Start() {

        // Change HP text when value is changed
        hp.SubscribeToText(hpText);

        // Add animation ( we use LeanTween for this because it's a simple library that just works )
        hp.Subscribe(_ => AnimateObj(hpText.gameObject));

        // HP Bar
        hp.Subscribe(currentHP => {
            float normalizedHP = (currentHP / 100f);
            LeanTween.scaleX(hpBar, normalizedHP, 0.2f)
                     .setEase(LeanTweenType.easeOutCirc);
        });
        
        // HP Delta 
        hp.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(delayedHP => {
            float normalizedHP = (delayedHP / 100f);
            LeanTween.scaleX(hpDelta, normalizedHP, 0.2f)
                     .setEase(LeanTweenType.easeInQuad);
        });
        
        // Button actions ------------------------
        damageButton.onClick.AddListener(DamageHP);
        resetButton.onClick.AddListener(ResetHP);
    }

    public void DamageHP() {
        hp.Value = Mathf.Max(0, hp.Value - UnityEngine.Random.Range(5, 30));
    }

    public void ResetHP() {
        hp.Value = 100;
    }

    public void AnimateObj(GameObject go) {
        LeanTween.scale(go, Vector3.one, 0.2f)
            .setFrom(Vector3.one*0.5f)
            .setEase(LeanTweenType.easeOutBack);
    }
}
