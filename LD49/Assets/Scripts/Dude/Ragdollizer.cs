using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdollizer : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void enable() {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        foreach (var coll in colliders)
        {
            coll.enabled = true;
        }
    }

    private void disable() {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        foreach (var coll in colliders)
        {
            coll.enabled = false;
        }
    }
}
