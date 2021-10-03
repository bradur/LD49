using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour
{
    private Dude dude;

    // Start is called before the first frame update
    void Start()
    {
        dude = GetComponentInChildren<Dude>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drink()
    {
        var newAmount = GameManager.Main.Drink();
        dude.SetDrinkAmount(newAmount);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Powerup")
        {
            var newAmount = GameManager.Main.Pickup(); //TODO: pickup what?
            dude.SetDrinkAmount(newAmount);
            Destroy(collider.gameObject);
        }
    }
}
