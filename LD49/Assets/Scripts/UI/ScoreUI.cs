using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{

    public static ScoreUI main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Text txtValue;

    [SerializeField]
    private Gradient scoreGradient; 

    [SerializeField]
    private int maxScoreForGradient = 5000;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float animationDuration = 0.5f;

    private int targetValue;
    private int valueAnimated;
    private int originalValue;

    private bool isAnimating = false;

    private float timer = 0;
    private int score = 0;

    public int Score { get { return score; } }

    private bool animatorAnimating = false;
    private Animator animator;

    public void Animate()
    {
        if (!animatorAnimating)
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator != null)
            {
                animator.Play("newScore");
                animatorAnimating = true;
            }
            else
            {
                Debug.LogWarning("No animator found for UIScore!");
            }
        }
    }

    public void FinishedAnimating()
    {
        animatorAnimating = false;
    }

    public void AddValueAnimated(int value)
    {
        if (!isAnimating)
        {
            Animate();
            timer = 0f;
            isAnimating = true;
            originalValue = score;
        }
        score += value;
        targetValue = score;
        txtValue.color = scoreGradient.Evaluate((float)(1.0f * score) / (float)maxScoreForGradient);
    }

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;

            valueAnimated = (int)Mathf.Lerp(originalValue, targetValue, timer / animationDuration);

            txtValue.text = valueAnimated.ToString("D8");

            if (timer > animationDuration)
            {
                txtValue.text = targetValue.ToString("D8");
                isAnimating = false;
            }
        }
    }

}
