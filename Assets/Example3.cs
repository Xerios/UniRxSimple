using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Example3 : MonoBehaviour
{

    public Button addButton, resetButton;

    public Text scoreText, deltaScoreText;

    public IntReactiveProperty score = new IntReactiveProperty(0);

    // Use this for initialization
    void Start () {

        // Score Delta 
        var scoreDelayed = score.Throttle(TimeSpan.FromMilliseconds(500)).ToReactiveProperty();
        var scoreDelta = score.Select(x => x - scoreDelayed.Value);

        // Change deltaScoreText and format it so that if the number is positive it has a "+" in front of it
        scoreDelta.SubscribeToText(deltaScoreText, x => (x > 0 ? ("+" + x) : x.ToString()));

        // Add animation everytime score delta changes
        scoreDelta.Subscribe(_ => AnimateObj(deltaScoreText.gameObject));


        scoreDelayed.Subscribe(delayedScore => {
            // Change text
            scoreText.text = delayedScore.ToString(); // You can also set by doing scoreDelayed.SubscribeToText(scoreText);

            // Start animation
            AnimateObj(scoreText.gameObject);

            // Clear deltaScore ocne we've updated the score
            deltaScoreText.text = "";
        });


        // Button actions ------------------------
        addButton.onClick.AddListener(() => score.Value++);
        resetButton.onClick.AddListener(() => score.Value=0);
    }

    public void AnimateObj(GameObject go) {
        LeanTween.scale(go, Vector3.one, 0.2f)
            .setFrom(Vector3.one*0.5f)
            .setEase(LeanTweenType.easeOutBack);
    }
}
