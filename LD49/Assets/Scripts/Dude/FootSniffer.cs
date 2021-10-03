using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootSniffer : MonoBehaviour
{

    [SerializeField]
    private UnityEvent<BananaSlipEvent> bananaSlipEvent;

    [SerializeField]
    private UnityEvent obstacleHitEvent;

    private int bananaLayer, obstacleLayer;

    // Start is called before the first frame update
    void Start()
    {
        bananaLayer = LayerMask.NameToLayer("Banana");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == bananaLayer) {
            var slipEvent = new BananaSlipEvent();
            slipEvent.Foot = this;
            slipEvent.Banana = other.gameObject;
            bananaSlipEvent.Invoke(slipEvent);
        }
        if (other.gameObject.layer == obstacleLayer) {
            obstacleHitEvent.Invoke();
        }
    }
}

public struct BananaSlipEvent {
    public FootSniffer Foot;
    public GameObject Banana;
}