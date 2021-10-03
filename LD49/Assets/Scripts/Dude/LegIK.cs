using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegIK : MonoBehaviour
{
    [SerializeField]
    private float maxFootStretchDistance = 2.0f;

    [SerializeField]
    private Transform footTransform;

    [SerializeField]
    private Transform optimalSpot;

    [SerializeField]
    private Transform ikTarget;
    
    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private AvatarIKGoal ikGoal;

    [HideInInspector]
    public IKOrchestrator Orchestrator;

    private bool allowedToMove = true;
    private bool moving = false;
    private Vector3 footMovingStartPosition;
    private float footStartedMoving;
    private float footStartedMovingVelocity;

    [SerializeField]
    private float footMoveSpeed = 25.0f;

    private Vector3 optimalSpotOffset;
    private Quaternion footTargetRotation;
    private int groundMask;

    [SerializeField]
    private Vector3 ikTargetGroundOffset = new Vector3(0, 1.0f, 0);

    private Vector3 velocity;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        ikTarget.parent = null;
        ikTarget.position = footTransform.position;

        optimalSpotOffset = optimalSpot.localPosition;
        optimalSpot.parent = transform.parent;

        groundMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        //optimalSpot.localPosition = optimalSpotOffset;

        var optimalGroundSpot = optimalSpot.position;
        RaycastHit hit;
        if (Physics.Raycast(optimalSpot.position, Vector3.down, out hit, 100.0f, groundMask)) {
            optimalGroundSpot = hit.point;
        }

        if (Vector3.Distance(optimalGroundSpot, footTransform.position) > maxFootStretchDistance){
            if (!moving && allowedToMove) {
                moving = true;
                Orchestrator.SetMovingLeg(this);
                ikTarget.position = footTransform.position;
                footMovingStartPosition = ikTarget.position;
                var randomAngle = Random.Range(-25f, 25f);
                footTargetRotation = optimalSpot.rotation * Quaternion.AngleAxis(randomAngle, Vector3.up);
                footStartedMoving = Time.time;
                footStartedMovingVelocity = velocity.magnitude;
            }
        }

        if (moving) {
            var velocityMod = Mathf.Max(footStartedMovingVelocity, 1.0f);
            var t = (Time.time - footStartedMoving) * velocityMod * 0.4f;
            t = Mathf.Clamp(t, 0.0f, 1.0f);

            var liftStrength = -(Mathf.Abs(t - 0.5f) * 2.0f - 1.0f);
            var horizPos = Vector3.Lerp(footMovingStartPosition, optimalGroundSpot, t);
            var vertPos = 1.5f * Vector3.up * liftStrength;
            ikTarget.position = horizPos + vertPos;

            ikTarget.rotation = Quaternion.RotateTowards(ikTarget.rotation, footTargetRotation, 180f * Time.deltaTime);
            
            if (t > 0.99999f){
                moving = false;
                Orchestrator.SetMovingLeg(null);
            }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(ikGoal, 1.0f);
        animator.SetIKRotationWeight(ikGoal, 1.0f);
        animator.SetIKPosition(ikGoal, ikTarget.position + ikTargetGroundOffset);
        animator.SetIKRotation(ikGoal, ikTarget.rotation);
    }

    public void AllowedToMove(bool allowed) {
        allowedToMove = allowed;
    }

}
