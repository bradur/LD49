using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    private Vector3 origPosition;

    [SerializeField]
    private int score = 10;

    public int Score { get { return score; } }


    // Start is called before the first frame update
    void Start()
    {
        origPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 180.0f);
        transform.position = origPosition + Vector3.up * Mathf.Sin(Time.time * 2.0f) * 1.0f;
    }
}
