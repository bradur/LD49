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

    private FillableBottle bottle;

    private Ragdollizer ragdoll;

    private float targetSway;
    private float curSway;
    private float swaySpeed = 10f;
    private float maxSwayRotation = 40f;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdollizer>();
        bottle = GetComponentInChildren<FillableBottle>();
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
    }

    public void Drink() {
        DrinkEvent.Invoke();
    }

    public void SetDrinkAmount(float percentage) {
        bottle.SetFillAmount(percentage);
    }

    public void SetSway(float amount) { // [-1.0, 1.0]
        amount = Mathf.Clamp(amount, -1.0f, 1.0f);
        targetSway = amount;
    }

    private void die() {
        DieEvent.Invoke();
        MoveAlongRoad.main.Stop();
        SidewaysMovement.main.Stop();
    }
}
