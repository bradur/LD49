using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ragdollizer : MonoBehaviour
{
    private List<Rigidbody> ragdollRigidbodies;
    private List<Collider> ragdollColliders;
    private Animator animator;

    [SerializeField]
    private bool manualTrigger;

    private Vector3 velocity;
    private Vector3 lastPosition;

    public bool Debug = false;

    public GameObject bottle;

    private int footSnifferLayer;

    // Start is called before the first frame update
    void Start()
    {
        footSnifferLayer = LayerMask.NameToLayer("FootSniffer");
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        ragdollColliders = GetComponentsInChildren<Collider>().Where(it => it.gameObject.layer != footSnifferLayer).ToList();
        animator = GetComponent<Animator>();
        disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug && !manualTrigger) {
            var movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            transform.Translate(movement * Time.deltaTime * 10.0f, Space.Self);
        }
        
        velocity = (transform.position - lastPosition) / Time.deltaTime;

        if (Debug && Input.GetKeyDown(KeyCode.Space)) {
            manualTrigger = !manualTrigger;
        }

        if (manualTrigger) {
            Enable();
            manualTrigger = false;
        }

        lastPosition = transform.position;
    }

    public void Enable() {
        Enable(1.0f);
    }
    public void Enable(float forceScale) {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce((velocity * 0.5f + Vector3.down * 10f) * forceScale, ForceMode.VelocityChange);
        }
        foreach (var coll in ragdollColliders)
        {
            coll.enabled = true;
        }
        animator.enabled = false;

        if (bottle != null) {
            bottle.transform.parent = null;
            var rb = bottle.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce((velocity + Vector3.up * 5f) * forceScale, ForceMode.VelocityChange);

            var coll = bottle.GetComponent<Collider>();
            coll.enabled = true;
        }
    }

    private void disable() {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        foreach (var coll in ragdollColliders)
        {
            coll.enabled = false;
        }
        animator.enabled = true;
    }

    public void Slip(FootSniffer foot, GameObject banana) {
        Enable(0.0f);

        var slipDirection = banana.transform.position - foot.transform.position;
        slipDirection.y = 0.0f;
        slipDirection.Normalize();

        var footRb = foot.transform.parent.parent.GetComponent<Rigidbody>();
        footRb.AddForce(slipDirection * 20.0f + Vector3.up * 1.5f, ForceMode.VelocityChange);
        var bananaRb = banana.GetComponent<Rigidbody>();
        bananaRb.AddForce(slipDirection * 10.0f + Vector3.up * 5f, ForceMode.VelocityChange);
    }
}
