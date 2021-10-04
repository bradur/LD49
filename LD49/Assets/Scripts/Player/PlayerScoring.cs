using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour
{
    private Dude dude;

    [SerializeField]
    private SoundManager soundManager;

    private int homeDoor, homeWall;


    // Start is called before the first frame update
    void Start()
    {
        dude = GetComponentInChildren<Dude>();

        homeDoor = LayerMask.NameToLayer("Home Door");
        homeWall = LayerMask.NameToLayer("Home Wall");
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
            Powerup powerup = collider.gameObject.GetComponentInChildren<Powerup>();
            GameManager.Main.AddScore(powerup.Score);
            var newAmount = GameManager.Main.Pickup(powerup.BeerAmount); //TODO: pickup what?
            dude.SetDrinkAmount(newAmount);
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.layer == homeDoor) {
            var door = collider.gameObject.GetComponent<CottageDoor>();
            door.Open();
            dude.Victory();
            Debug.Log("GOOD WIN!");
            Invoke("GoodEnd", 4f);
        }
        if (collider.gameObject.layer == homeWall) {
            dude.Victory();
            Debug.Log("BAD WIN!");
            Invoke("BadEnd", 4f);
        }
        if (collider.gameObject.tag == "MusicTrigger") {
            RoadBiomeConfig biome = collider.gameObject.GetComponentInParent<MusicTrigger>().Config;
            if (biome != null && biome.Music != null) {
                Debug.Log($"Playing music: {biome.Music}");
                MusicPlayer.main.PlayMusic(biome.Music);
            }
            Destroy(collider.gameObject);
        }
    }

    private void GoodEnd() {
        UIMenu.main.ShowTheEndMenu("You made it back home safely!");
    }

    private void BadEnd() {
        UIMenu.main.ShowTheEndMenu("You got home! Kinda...");
    }
}
