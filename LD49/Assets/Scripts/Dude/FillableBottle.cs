using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillableBottle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer rend;
    
    [SerializeField]
    private float minFill = 0.0f;
    
    [SerializeField]
    private float maxFill = 1.0f;

    [SerializeField]
    private float lerpTime = 1.0f;

    private float lerpStarted = -1.0f;
    private float targetAmount = 1.0f;
    private float startAmount = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rend.material.SetFloat("_FillAmount", maxFill);
        Debug.Log(rend.material);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpStarted >= 0.0f) {
            var t = (Time.time - lerpStarted) / lerpTime;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            var fillAmount = Mathf.Lerp(startAmount, targetAmount, t);
            fillAmount = Mathf.Lerp(minFill, maxFill, fillAmount);
            rend.material.SetFloat("_FillAmount", fillAmount);
        }
    }

    public void SetFillAmount(float amount) {
        startAmount = targetAmount;
        targetAmount = amount;
        lerpStarted = Time.time;
    }
}
