using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceCursor : MonoBehaviour
{
    [SerializeField]
    private float maxAngle; // maximum euler angle for the cursor, eg. 60
    [SerializeField]
    private float minAngle; // maximum euler angle for the cursor, eg. -60
    [SerializeField]
    private float minRandomValue; 
    [SerializeField]
    private float maxRandomValue;

    RectTransform rectTransform;
    float value = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // float max = value > 0 ? maxAngle : minAngle; // max is the largest value that abs(value) can have
        // float randomCoef = value/max * (maxRandomValue - minRandomValue) + minRandomValue; 
        // value += (Mathf.PerlinNoise(Time.time, 0.0f) * 2f - 1f) * randomCoef;
        // Debug.Log((Mathf.PerlinNoise(Time.time, 0.0f) * 2f - 1f));
        value = value > 0 ? Mathf.Min(value, maxAngle) : Mathf.Max(value, minAngle);
        rectTransform.rotation = Quaternion.Euler(0f, 0f, value);
    }

    public void SetBalance(float balance) {
        value = balance;
    }
}
