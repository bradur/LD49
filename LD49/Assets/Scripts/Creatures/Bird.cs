using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float walkDuration = 2f;
    private float walkStarted = 0f;
    private float rotation = 0f;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - walkStarted > walkDuration)
        {
            rotation = rotation < 180f ? 180f : 0f;
            body.rotation = Quaternion.Euler(0, rotation, 0);
            walkStarted = Time.time;
            body.velocity = Vector3.zero;
        }
    }

    public void Walk()
    {
        body.velocity = transform.forward * walkSpeed;
    }
    
    public void Stop()
    {
        body.velocity = Vector3.zero;
    }
}
