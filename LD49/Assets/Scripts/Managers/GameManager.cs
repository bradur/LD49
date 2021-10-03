using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BeerConfig beerConfig;

    [SerializeField]
    private float startLevelDelay = 1f;


    private float beerAmount = 0f;
    private float score = 0f;

    public static GameManager Main
    {
        get; private set;
    }

    void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Main = this;
        }
    }

    void Start()
    {
        UIFullscreenFade.main.FadeIn(delegate
        {
            Invoke("StartLevel", startLevelDelay);
        });

        beerAmount = beerConfig.BeerTotal;
    }
    
    public void StartLevel() {
        GenerateRoad.main.Begin();
        UIFullscreenFade.main.FadeOut(delegate
        {
            CameraManager.main.SetUp(delegate {
                BiomeManager.main.Begin();
                MoveAlongRoad.main.Begin();
                SidewaysMovement.main.Begin();
            });
        });
    }

    public void Update() {
        BeerMeter.main.SetAmount(beerAmount/beerConfig.BeerTotal);
    }

    public float Drink() {
        beerAmount -= beerConfig.BeerSipAmount;
        score += beerConfig.BeerSipScore * Mathf.Lerp(
            beerConfig.MinBeerScoreModifier,
            beerConfig.MaxBeerScoreModifier,
            beerAmount/beerConfig.BeerTotal
        );

        return beerAmount/beerConfig.BeerTotal;
    }

    public float Pickup() {
        beerAmount = Mathf.Min(beerAmount + beerConfig.BeerPickupAmount, beerConfig.BeerTotal);
        return beerAmount;
    }

    public float GetScore() {
        return score;
    }
}
