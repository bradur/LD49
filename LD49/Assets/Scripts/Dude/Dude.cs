using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dude : MonoBehaviour
{
    [SerializeField]
    private UnityEvent DieEvent;

    private Ragdollizer ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdollizer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Slip(BananaSlipEvent slipEvent) {
        ragdoll.Slip(slipEvent.Foot, slipEvent.Banana);
        die();
    }

    private void die() {
        DieEvent.Invoke();
    }
}
