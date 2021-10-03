using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour
{
    private Dude dude;

    [SerializeField]
    private SoundManager soundManager;


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
        soundManager.PlaySound(GameSoundType.DrinkBeer);
        var newAmount = GameManager.Main.Drink();
        dude.SetDrinkAmount(newAmount);

        if (newAmount < 0.001f) {
            dude.Exhaust();
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Powerup")
        {
            soundManager.PlaySound(GameSoundType.PickupBeer);
            var newAmount = GameManager.Main.Pickup(); //TODO: pickup what?
            dude.SetDrinkAmount(newAmount);
            Destroy(collider.gameObject);
        }
    }
}
