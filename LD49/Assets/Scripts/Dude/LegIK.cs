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

    [SerializeField]
    private float footMoveSpeed = 25.0f;

    private Vector3 optimalSpotOffset;
    private int groundMask;

    [SerializeField]
    private Vector3 ikTargetGroundOffset = new Vector3(0, 1.0f, 0);

    // Start is called before the first frame update
    void Start()
    {
        ikTarget.parent = transform.parent;
        ikTarget.position = footTransform.position;

        optimalSpotOffset = optimalSpot.position - transform.position;
        optimalSpot.parent = transform.parent;

        groundMask = LayerMask.GetMask("Ground", "Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        optimalSpot.position = transform.position + optimalSpotOffset;

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
            }
        }

        if (moving) {
            if (Vector3.Distance(optimalGroundSpot, ikTarget.position) < 0.1f){
                moving = false;
                Orchestrator.SetMovingLeg(null);
            } else {
                ikTarget.position = Vector3.MoveTowards(ikTarget.position, optimalGroundSpot, footMoveSpeed * Time.deltaTime);
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
