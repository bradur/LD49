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

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdollizer>();
        bottle = GetComponentInChildren<FillableBottle>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Slip(BananaSlipEvent slipEvent) {
        ragdoll.Slip(slipEvent.Foot, slipEvent.Banana);
        die();
    }

    private float drinkAmount = 1.0f;

    public void Drink() {
        DrinkEvent.Invoke();

        //TODO: get rid of this once it's handled externally
        drinkAmount = drinkAmount - 0.25f;
        SetDrinkAmount(drinkAmount);
    }

    public void SetDrinkAmount(float percentage) {
        bottle.SetFillAmount(percentage);
    }

    private void die() {
        DieEvent.Invoke();
        MoveAlongRoad.main.Stop();
        SidewaysMovement.main.Stop();
    }
}
