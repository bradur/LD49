using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootSniffer : MonoBehaviour
{

    [SerializeField]
    private UnityEvent<BananaSlipEvent> bananaSlipEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        var slipEvent = new BananaSlipEvent();
        slipEvent.Foot = this;
        slipEvent.Banana = other.gameObject;
        bananaSlipEvent.Invoke(slipEvent);
    }
}

public struct BananaSlipEvent {
    public FootSniffer Foot;
    public GameObject Banana;
}