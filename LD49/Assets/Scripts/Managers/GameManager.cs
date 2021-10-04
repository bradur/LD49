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
    private int score = 0;

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
        BiomeManager.main.Begin();
        PowerupManager.main.Begin();
        GenerateRoad.main.Begin();
        UIFullscreenFade.main.FadeOut(delegate
        {
            CameraManager.main.SetUp(delegate {
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
        int addedScore = (int) (beerConfig.BeerSipScore * Mathf.Lerp(
            beerConfig.MinBeerScoreModifier,
            beerConfig.MaxBeerScoreModifier,
            beerAmount/beerConfig.BeerTotal
        ));
        AddScore(addedScore);
        return beerAmount/beerConfig.BeerTotal;
    }

    public void AddScore(int addedScore) {
        ScoreUI.main.AddValueAnimated(addedScore);
        score += addedScore;
    }

    public float Pickup() {
        beerAmount = Mathf.Min(beerAmount + beerConfig.BeerPickupAmount, beerConfig.BeerTotal);
        return beerAmount;
    }

    public int GetScore() {
        return score;
    }
}
