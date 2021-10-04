using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dude : MonoBehaviour
{
    [SerializeField]
    private UnityEvent DieEvent;

    [SerializeField]
    private UnityEvent DrinkEvent;

    [SerializeField]
    private UnityEvent BumpEvent;

    private FillableBottle bottle;

    private Ragdollizer ragdoll;

    private float targetSway;
    private float curSway;
    private float swaySpeed = 10f;
    private float maxSwayRotation = 40f;

    private Animator animator;
    private IKOrchestrator iKOrchestrator;

    private int fullBodyLayer, upperbodyLayer, legsLayer;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdollizer>();
        bottle = GetComponentInChildren<FillableBottle>();
        animator = GetComponent<Animator>();
        iKOrchestrator = GetComponent<IKOrchestrator>();

        fullBodyLayer = animator.GetLayerIndex("Full Body");
        upperbodyLayer = animator.GetLayerIndex("Upper Body");
        legsLayer = animator.GetLayerIndex("IK Pass");
    }

    // Update is called once per frame
    void Update()
    {
        if (curSway != targetSway) {
            var swayDiff = targetSway - curSway;
            var swayAmount = Mathf.Min(swayDiff, swaySpeed * Time.deltaTime);
            curSway += swayAmount;
        }
        transform.localRotation = Quaternion.AngleAxis(maxSwayRotation * curSway, Vector3.back);
    }

    public void Slip(BananaSlipEvent slipEvent) {
        ragdoll.Slip(slipEvent.Foot, slipEvent.Banana);
        die();
    }

    public void ObstacleHit() {
        ragdoll.Enable();
        die();
        Invoke("FallGameOver", 3f);
    }

    public void Victory() {
        ragdoll.Enable();
        iKOrchestrator.Stop();
        MoveAlongRoad.main.Stop();
        SidewaysMovement.main.Stop();
    }

    public void Drink() {
        DrinkEvent.Invoke();
    }

    public void BumpHit() {
        BumpEvent.Invoke();
    }

    public void SetDrinkAmount(float percentage) {
        bottle.SetFillAmount(percentage);
    }


    public void Exhaust() {
        SetSway(0);
        animator.SetBool("Exhausted", true);
        animator.SetLayerWeight(fullBodyLayer, 1.0f);
        animator.SetLayerWeight(upperbodyLayer, 0.0f);
        animator.SetLayerWeight(legsLayer, 0.0f);
        animator.Play("Exhaust", fullBodyLayer, 0.0f);
        die();
        Invoke("ExhaustGameOver", 7f);
    }

    public void ReleaseBottle() {
        ragdoll.ReleaseBottle();
    }

    public void SetSway(float amount) { // [-1.0, 1.0]
        amount = Mathf.Clamp(amount, -1.0f, 1.0f);
        targetSway = amount;
    }

    private void die() {
        iKOrchestrator.Stop();
        DieEvent.Invoke();
        MoveAlongRoad.main.Stop();
        SidewaysMovement.main.Stop();
    }

    private void ExhaustGameOver() {
        UIMenu.main.ShowGameOverMenu("You ran out of beer!");
    }

    private void FallGameOver() {
        UIMenu.main.ShowGameOverMenu("You walked off the road!");
    }
}
