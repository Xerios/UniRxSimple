using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Example1 : MonoBehaviour
{

    public Button addButton;

    public Text scoreText;

    // We use IntReactiveProperty so that we can see and modify the value in the Inspector, other than that it's the same as ReactiveProperty<Int>
    public IntReactiveProperty score = new IntReactiveProperty(0);

    // Use this for initialization
    void Start () {

        // Change text when score is changed
        score.SubscribeToText(scoreText);

        // Add animation ( we use LeanTween for this because it's a simple library that just works )
        score.Subscribe(_ => LeanTween.scale(scoreText.gameObject, Vector3.one, 0.2f).setFrom(Vector3.one*0.5f).setEase(LeanTweenType.easeOutBack));

        /* 
         // Two above lines can also be written as follows :
         score.Subscribe(x => {
              scoreText.Text = x.toString();
              LeanTween.scale(scoreText.gameObject, Vector3.one, 0.2f).setFrom(Vector3.one*0.5f).setEase(LeanTweenType.easeOutBack)
         });
         */

        // Button actions ------------------------
        addButton.onClick.AddListener(() => {
            score.Value++;
        });
    }
}
